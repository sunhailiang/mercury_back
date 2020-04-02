using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QRCodeManager.Authorizer;
using QRCodeManager.ConfigModels;
using QRCodeManager.Models;
using QRCodeManager.Models.QRCodeSeller;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [CustomAuthorize(Permission = "QRCodeSeller")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly LitemallContext litemallContext;
        private readonly QRCodeSellerContext sellerContext;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClient;

        private int AvailableSellerID(bool isCA, bool isMA)
        {
            int sellerID = GetCurrentSeller()?.SellerId ?? 0;
            int newID;
            bool isAvailable = false;
            do
            {
                newID = BitConverter.ToInt32(RandomID(), 0);
                if (isCA && sellerContext.QrcodeSellerList.FirstOrDefault(x => BitConverter.GetBytes(x.SellerId)[0] == BitConverter.GetBytes(newID)[0]) != default)
                {
                    continue;
                }

                if (isMA && sellerContext.QrcodeSellerList.FirstOrDefault(x => BitConverter.GetBytes(x.SellerId)[0] == BitConverter.GetBytes(newID)[0] && BitConverter.GetBytes(x.SellerId)[1] == BitConverter.GetBytes(newID)[1]) != default)
                {
                    continue;
                }

                if (sellerContext.QrcodeSellerList.FirstOrDefault(x => x.SellerId == newID) != default)
                {
                    continue;
                }

                isAvailable = true;
            } while (newID == 0 || !isAvailable);
            return newID;

            //产生符合规则随机ID的内联方法
            byte[] RandomID()
            {
                byte[] idBytes = BitConverter.GetBytes(sellerID);
                Random random = new Random();
                byte agencyCode = idBytes[0];
                byte managerCode = idBytes[1];
                random.NextBytes(idBytes);

                //为普通用户和MA保留agencyCode
                if (!isCA)
                {
                    idBytes[0] = agencyCode;
                }

                //为普通用户保留managerCode
                if (!isMA && !isCA)
                {
                    idBytes[1] = managerCode;
                }

                return idBytes;
            }
        }

        private QrcodeSellerList GetCurrentSeller()
        {
            Guid currentUserGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "guid").Value);
            return sellerContext.QrcodeSellerList.FirstOrDefault(x => x.UserGuid == currentUserGuid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey">根据当前的UserID查询出来的SellerId</param>
        /// <param name="currentSellerGuid">当前用户的UserID</param>
        /// <returns></returns>
        private bool IsChild(Guid? groupKey, Guid? currentSellerGuid)
        {
            //半夏是根节点，总能看到所有人
            if (currentSellerGuid == Guid.Parse("FC45FBF4-1CE4-4422-96B6-FEFBAEBAFFC6"))
            {
                return true;
            }

            Guid? ca = groupKey;
            while (ca != Guid.Parse("FC45FBF4-1CE4-4422-96B6-FEFBAEBAFFC6"))
            {
                if (ca == currentSellerGuid)
                {
                    return true;
                }
                else
                {
                    ca = sellerContext.QrcodeSellerList.FirstOrDefault(x => x.UserGuid == ca).CA;
                }
            }
            return false;
        }

        public SellerController(LitemallContext litemallDB, QRCodeSellerContext sellerDB, IConfiguration config, IHttpClientFactory httpClient)
        {
            litemallContext = litemallDB;
            sellerContext = sellerDB;
            configuration = config;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// 新建分销码签出记录
        /// </summary>
        /// <param name="newSeller"></param>
        /// <returns></returns>
        [HttpPost("MA")]
        [CustomAuthorize(Permission = "QRCodeSellerCA")]
        public IActionResult Create([FromBody]QrcodeSellerList body, bool isMA = true)
        {
            IQueryable<QrcodeSellerList> exsit = from user in sellerContext.QrcodeSellerList
                                                 where user.PhoneNumber == body.PhoneNumber && user.Type == body.Type
                                                 select user;
            QrcodeSellerList exsitSeller = exsit.FirstOrDefault();
            if (exsitSeller != default)
            {
                if (exsitSeller.IsEnable)
                {
                    //每个用户同类分销码最多只可以有一个
                    return Forbid();
                }
                else
                {
                    sellerContext.QrcodeSellerList.Remove(exsitSeller);
                    sellerContext.SaveChanges();
                    return Create(body, isMA);
                }
            }

            using (HttpClient client = httpClient.CreateClient())
            {
                QrcodeSellerList currentSeller = GetCurrentSeller();

                //AutoCreate
                string jupiterPath = configuration.GetSection("AppSetting").Get<JupiterKeys>().JupiterPath;
                StringContent content = new StringContent(JsonConvert.SerializeObject(new { body.PhoneNumber, WeChatUnionID = default(string), Permission = isMA ? "QRCodeSellerMA" : "QRCodeSeller" }));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                Task<HttpResponseMessage> x = client.PostAsync($"{jupiterPath}/api/v1/User/AutoCreate", content);
                body.SellerId = AvailableSellerID(BitConverter.GetBytes(currentSeller.SellerId)[0] == 0 && isMA, isMA);
                body.CreateTime = DateTime.Now;
                body.ModifyTime = DateTime.Now;
                body.IsEnable = true;
                body.CA = currentSeller?.UserGuid ?? Guid.Empty;
                string guidStr = JsonConvert.DeserializeObject<dynamic>(x.Result.Content.ReadAsStringAsync().Result).data.guid;
                var guid = Guid.Parse(guidStr);
                //Guid guid = Guid.Parse(JToken.Parse(x.Result.Content.ReadAsStringAsync().Result.Trim()).Value<string>());
                IQueryable<QrcodeSellerList> belongOtherCA = from record in sellerContext.QrcodeSellerList
                                                             where record.UserGuid == guid
                                                             where record.CA != body.CA
                                                             where record.IsEnable == true
                                                             select record;

                if (belongOtherCA.FirstOrDefault() != default)
                {
                    return Forbid();
                }

                body.UserGuid = guid;
                sellerContext.QrcodeSellerList.Add(body);
                sellerContext.SaveChanges();
            }
            return Ok(body);
        }

        [CustomAuthorize(Permission = "QRCodeSellerMA")]
        public IActionResult Create([FromBody]QrcodeSellerList body)
        {
            return Create(body, false);
        }


        /// <summary>
        /// 获取指定分销ID的相关信息
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        [HttpGet("{SellerId}")]
        [CustomAuthorize(Permission = "QRCodeSellerMA")]
        public IActionResult QRCodeSeller(int SellerId)
        {
            QrcodeSellerList currentSeller = GetCurrentSeller();
            IQueryable<QrcodeSellerList> target = from seller in sellerContext.QrcodeSellerList
                                                  where seller.SellerId == SellerId
                                                  where seller.IsEnable == true
                                                  where seller.CA == currentSeller.CA
                                                  select seller;
            return Ok(target.First());
        }

        /// <summary>
        /// 返回当前用户拥有的全部SellerID
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult QRCodeSeller()
        {
            Guid currentUserGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "guid").Value);
            IQueryable<QrcodeSellerList> targets = from seller in sellerContext.QrcodeSellerList
                                                   where seller.UserGuid == currentUserGuid
                                                   where seller.IsEnable == true
                                                   select seller;
            return Ok(targets);
        }

        /// <summary>
        /// 返回当前MA管理的全部SellerID
        /// </summary>
        /// <param name="isEnable"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission = "QRCodeSellerMA")]
        public IActionResult List(string type = "Basic", bool isEnable = true)
        {
            QrcodeSellerList currentSeller = GetCurrentSeller();
            IQueryable<IGrouping<Guid, QrcodeSellerList>> enableSeller = from seller in sellerContext.QrcodeSellerList
                                                                         where seller.IsEnable == isEnable
                                                                         where seller.Type == type || type == "Any"
                                                                         group seller by seller.CA;
            List<QrcodeSellerList> target = new List<QrcodeSellerList>();
            var rates = from com in sellerContext.Commission
                        orderby com.Mtime descending
                        select new { com.Rate, com.UserGuid };
            foreach (IGrouping<Guid, QrcodeSellerList> group in enableSeller)
            {
                if (IsChild(group.Key, currentSeller.UserGuid))
                {
                    target.AddRange(group);
                }
            }
            var result = from seller in target
                         select new { seller.IdentityCardNumber, seller.PhoneNumber, seller.IsEnable, seller.Name, SellerID = seller.SellerId, seller.Type, Rate = rates.FirstOrDefault(x => x.UserGuid == seller.UserGuid)?.Rate ?? 0.5 };
            return Ok(result);
        }

        /// <summary>
        /// 分销码的修改，只有身份证号和姓名会被更新
        /// </summary>
        /// <param name="qRCodeManger">要修改的数据</param>
        /// <param name="sellerid">要修改的筛选条件字段</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission = "QRCodeSellerMA")]
        public IActionResult Update([FromBody]QrcodeSellerList body)
        {
            QrcodeSellerList currentSeller = GetCurrentSeller();
            IQueryable<QrcodeSellerList> target = from seller in sellerContext.QrcodeSellerList
                                                  where seller.SellerId == body.SellerId
                                                  where seller.CA == currentSeller.CA || currentSeller.CA == Guid.Parse("FC45FBF4-1CE4-4422-96B6-FEFBAEBAFFC6")
                                                  select seller;
            QrcodeSellerList updateTarget = target.First();
            updateTarget.IdentityCardNumber = body.IdentityCardNumber;
            updateTarget.Name = body.Name;
            updateTarget.IsEnable = body.IsEnable;
            updateTarget.ModifyTime = DateTime.Now;
            sellerContext.Update(updateTarget);
            sellerContext.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// 获取指定分销员关联订单
        /// </summary>
        /// <param name="sellerID"></param>
        /// <param name="type"></param>
        /// <returns></returns>

        [HttpGet("{SellerId}")]
        [CustomAuthorize(Permission = "QRCodeSellerMA")]
        public IActionResult Orders(int sellerID, string type = "Basic")
        {
            Guid currentUserGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "guid").Value);
            QrcodeSellerList sellerGuid = sellerContext.QrcodeSellerList.FirstOrDefault(x => x.SellerId == sellerID);
            if (sellerGuid.UserGuid != Guid.Empty && !IsChild(sellerGuid.CA, currentUserGuid))
            {
                return Ok(Array.Empty<object>());
            }
            else
            {
                IQueryable<SellerUserMapping> relatedCostumers = from sum in sellerContext.SellerUserMapping
                                                                 where (sum.Type == type || type == "Any") && sum.SellerId == sellerID
                                                                 select sum;
                IEnumerable<LitemallOrder> relatedOrders = from sum in relatedCostumers.ToList()
                                                           from order in litemallContext.LitemallOrder
                                                           where order.UserId == sum.CostumerId
                                                           select order;
                LitemallOrder[] result = relatedOrders.ToArray();
                return Ok(result);
            }
        }

        /// <summary>
        /// 获取当前分销员的关联订单
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Orders(string type = "Basic")
        {
            Guid currentUserGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "guid").Value);
            IQueryable<QrcodeSellerList> currentIds = from seller in sellerContext.QrcodeSellerList
                                                      where seller.UserGuid == currentUserGuid
                                                      where type == "Any" || seller.Type == type
                                                      where seller.IsEnable == true
                                                      select seller;

            IQueryable<SellerUserMapping> relatedCostumers = from id in currentIds
                                                             from sum in sellerContext.SellerUserMapping
                                                             where sum.SellerId == id.SellerId
                                                             select sum;

            IEnumerable<LitemallOrder> relatedOrders = from sum in relatedCostumers.ToArray()
                                                       from order in litemallContext.LitemallOrder
                                                       where order.UserId == sum.CostumerId
                                                       select order;
            return Ok(relatedOrders);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult New([FromForm]int costumerID, [FromForm]int sellerID, [FromForm]string type = "Basic")
        {
            type = sellerContext.QrcodeSellerList.FirstOrDefault(x => x.SellerId == sellerID)?.Type;
            QrcodeSellerList message = sellerContext.QrcodeSellerList.FirstOrDefault(x => x.SellerId == sellerID);
            if (type == "Basic")
            {
                if ((from sum in sellerContext.SellerUserMapping
                     where sum.CostumerId == costumerID && sum.Type == type
                     select sum).FirstOrDefault() == default)
                {
                    sellerContext.SellerUserMapping.Add(new SellerUserMapping
                    {
                        CostumerId = costumerID,
                        SellerId = sellerID,
                        CreateTime = DateTime.Now,
                        Type = type
                    });
                    sellerContext.SaveChanges();
                    string x = AliyunSms.SendSms(message.PhoneNumber, message.Name, message.Type);
                    return Ok();
                }
                else
                {
                    return Forbid();
                }
            }
            else
            {
                sellerContext.SellerUserMapping.Add(new SellerUserMapping
                {
                    CostumerId = costumerID,
                    SellerId = sellerID,
                    CreateTime = DateTime.Now,
                    Type = type
                });
                sellerContext.SaveChanges();
                string x = AliyunSms.SendSms(message.PhoneNumber, message.Name, message.Type);
                return Ok();
            }
        }

        public static class AliyunSms
        {
            public static string SendSms(string phonenumber, string userName, string type)
            {
                string accessKeyId = "LTAI4FuyZFfh5R1z5NTuynpL";//你的accessKeyId，参考本文档步骤2
                string accessKeySecret = "KRFmpYlyAWdP3XiEdm5bhag3wFkelv";//你的accessKeySecret，参考本文档步骤2

                Dictionary<string, string> smsDict = new Dictionary<string, string>
                {
                    { "PhoneNumbers", phonenumber },
                    { "SignName", "滇峰" },
                    { "TemplateCode", "SMS_173477295" },
                    { "TemplateParam", JsonConvert.SerializeObject(new { userName, type }) },
                    { "RegionId", "cn-hangzhou" },
                    { "Action", "SendSms" },
                    { "Version", "2017-05-25" }
                };
                string domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
                SignatureHelper singnature = new SignatureHelper();
                return singnature.Request(accessKeyId, accessKeySecret, domain, smsDict);
            }

            public class SignatureHelper
            {
                private const string ISO8601_DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:ss'Z'";
                private const string ENCODING_UTF8 = "UTF-8";

                public static string PercentEncode(string value)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
                    byte[] bytes = Encoding.GetEncoding(ENCODING_UTF8).GetBytes(value);
                    foreach (char c in bytes)
                    {
                        if (text.IndexOf(c) >= 0)
                        {
                            stringBuilder.Append(c);
                        }
                        else
                        {
                            stringBuilder.Append("%").Append(
                                string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)c));
                        }
                    }
                    return stringBuilder.ToString();
                }

                public static string FormatIso8601Date(DateTime date)
                {
                    return date.ToUniversalTime().ToString(ISO8601_DATE_FORMAT, CultureInfo.CreateSpecificCulture("en-US"));
                }

                private static IDictionary<string, string> SortDictionary(Dictionary<string, string> dic)
                {
                    IDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dic, StringComparer.Ordinal);
                    return sortedDictionary;
                }

                public static string SignString(string source, string accessSecret)
                {
                    using (HMACSHA1 algorithm = new HMACSHA1())
                    {
                        algorithm.Key = Encoding.UTF8.GetBytes(accessSecret.ToCharArray());
                        return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(source.ToCharArray())));
                    }
                }

                public async Task<string> HttpGet(string url)
                {
                    string responseBody = string.Empty;
                    using (HttpClient http = new HttpClient())
                    {
                        try
                        {
                            http.DefaultRequestHeaders.Add("x-sdk-client", "Net/2.0.0");
                            HttpResponseMessage response = await http.GetAsync(url);
                            response.EnsureSuccessStatusCode();
                            responseBody = await response.Content.ReadAsStringAsync();
                            response.Dispose();
                        }
                        catch (HttpRequestException e)
                        {
                            Console.WriteLine("\nException !");
                            Console.WriteLine("Message :{0} ", e.Message);
                        }
                    }
                    return responseBody;
                }

                public string Request(string accessKeyId, string accessKeySecret, string domain, Dictionary<string, string> paramsDict, bool security = false)
                {

                    string result = string.Empty;
                    Dictionary<string, string> apiParams = new Dictionary<string, string>
                    {
                        { "SignatureMethod", "HMAC-SHA1" },
                        { "SignatureNonce", Guid.NewGuid().ToString() },
                        { "SignatureVersion", "1.0" },
                        { "AccessKeyId", accessKeyId },
                        { "Timestamp", FormatIso8601Date(DateTime.Now) },
                        { "Format", "JSON" }
                    };

                    foreach (KeyValuePair<string, string> param in paramsDict)
                    {
                        if (!apiParams.ContainsKey(param.Key))
                        {
                            apiParams.Add(param.Key, param.Value);
                        }
                    }
                    IDictionary<string, string> sortedDictionary = SortDictionary(apiParams);
                    string sortedQueryStringTmp = "";
                    foreach (KeyValuePair<string, string> param in sortedDictionary)
                    {
                        sortedQueryStringTmp += "&" + PercentEncode(param.Key) + "=" + PercentEncode(param.Value);
                    }
                    string stringToSign = "GET&%2F&" + PercentEncode(sortedQueryStringTmp.Substring(1));
                    string sign = SignString(stringToSign, accessKeySecret + "&");
                    string signature = PercentEncode(sign);
                    string url = (security ? "https" : "http") + $"://{domain}/?Signature={signature}{sortedQueryStringTmp}";
                    try
                    {
                        result = HttpGet(url).Result;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return result;

                }
            }
        }

        /// <summary>
        /// 分销佣金
        /// </summary>
        /// <param name="commissionViewModel">前端传递过来的实体数据模型</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission = "QRCodeSellerMA")]
        public IActionResult SetCommissionRate(CommissionViewModel commissionViewModel)
        {
            Guid currentUserGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "guid").Value);
            QrcodeSellerList guid = sellerContext.QrcodeSellerList.First(s => s.SellerId == commissionViewModel.SellerID);
            //权限验证
            if (IsChild(guid.UserGuid, currentUserGuid) && (sellerContext.Commission.FirstOrDefault(x => x.UserGuid == currentUserGuid)?.Rate ?? 0.5) >= commissionViewModel.Rate)
            {
                Commission commission = new Commission()
                {
                    Mtime = DateTime.Now,
                    Rate = commissionViewModel.Rate,
                    SellerId = commissionViewModel.SellerID,
                    UserGuid = guid.UserGuid,
                    CreatedBy = currentUserGuid,
                };
                sellerContext.Commission.Add(commission);
                sellerContext.SaveChanges();
                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        /// <summary>
        /// 计算器
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Calculator(string type = "Basic")
        {
            //查询当前用户的UserGuid
            Guid currentUserGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "guid").Value);

            IQueryable<QrcodeSellerList> currentIds = from seller in sellerContext.QrcodeSellerList
                                                      where seller.UserGuid == currentUserGuid
                                                      where type == "Any" || seller.Type == type
                                                      where seller.IsEnable == true
                                                      select seller;

            IQueryable<SellerUserMapping> relatedCostumers = from id in currentIds
                                                             from sum in sellerContext.SellerUserMapping
                                                             where sum.SellerId == id.SellerId
                                                             select sum;

            IEnumerable<LitemallOrder> relatedOrders = from sum in relatedCostumers.ToArray()
                                                       from order in litemallContext.LitemallOrder
                                                       where order.UserId == sum.CostumerId
                                                       where order.ConfirmTime != null
                                                       select order;

            IOrderedQueryable<Commission> commissionQuery = from commission in sellerContext.Commission
                                                            where commission.UserGuid == currentUserGuid
                                                            orderby commission.Mtime descending
                                                            select commission;
            decimal price = 0m;
            foreach (LitemallOrder item in relatedOrders)
            {
                double targetRate = commissionQuery.FirstOrDefault(x => x.Mtime <= item.ConfirmTime)?.Rate ?? 0.5;

                price += item.ActualPrice * (decimal)targetRate;

            }
            return Ok(Math.Round(price, 2));
        }

        [HttpGet]
        public IActionResult Rate()
        {
            Guid currentUserGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "guid").Value);
            IOrderedQueryable<Commission> commissionQuery = from commission in sellerContext.Commission
                                                            where commission.UserGuid == currentUserGuid
                                                            orderby commission.Mtime descending
                                                            select commission;
            double targetRate = commissionQuery.FirstOrDefault()?.Rate ?? 0.5;
            return Ok(targetRate);
        }


    }
}
