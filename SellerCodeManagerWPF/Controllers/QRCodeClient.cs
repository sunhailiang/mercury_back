using Newtonsoft.Json;
using SellerCodeManagerWPF.Controllers.Exceptions;
using SellerCodeManagerWPF.Models;
using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SellerCodeManagerWPF.Controllers
{
    class QRCodeClient
    {
        private const string apiUri = "https://qrcodemanager.clarabeautynism.com/Seller/";
        private const string getAccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx1cab85d8b1aa1ed5&secret=18d2f4b2bcb506b563e5992759119ca7";
        private static readonly HttpClient HttpClient = new HttpClient { BaseAddress = new Uri(apiUri) };

        /// <summary>
        /// 向腾讯后台申请指定sellerID参数的太阳码
        /// </summary>
        /// <param name="sellerID"></param>
        /// <returns></returns>
        internal static async Task GetQRCode(int sellerID,string savingPath)
        {
            using (var httpClient = new HttpClient())
            {
                dynamic result = JsonConvert.DeserializeObject(await httpClient.GetStringAsync(getAccessTokenUrl));
                string accessToken = result.access_token;
                using (var content = new StringContent(JsonConvert.SerializeObject(new { scene = $"recommendId,{sellerID}" })))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    using (var x = await httpClient.PostAsync($"https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={accessToken}", content))
                    {
                        Image.FromStream(await x.Content.ReadAsStreamAsync()).Save(savingPath);
                    }
                }
            }
        }

        /// <summary>
        /// 创建一个分销员
        /// </summary>
        /// <param name="identityCardNumber">身份证号</param>
        /// <param name="name">姓名</param>
        /// <param name="phoneNumber">电话号码</param>
        /// <param name="type">分销码类型</param>
        /// <param name="isMA">是否为管理员</param>
        /// <param name="rate">佣金比例</param>
        /// <exception cref="UnExceptedException"/>
        /// <returns></returns>
        internal static async Task<int> CreateSeller(string identityCardNumber, string name, string phoneNumber, string type, bool isMA, double rate)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JupiterClient.Token);

            if (type == "Any")
            {
                throw new UnExceptedException("Any是保留字，禁止生成type为Any的分销码");
            }

            if (isMA == true && JupiterClient.IsCA == false)
            {
                throw new UnExceptedException("权限不足，当前用户没有创建管理者的权限");
            }

            QRCodeSeller seller = new QRCodeSeller
            {
                IdentityCardNumber = identityCardNumber,
                Name = name,
                PhoneNumber = phoneNumber,
                Type = type,
                SellerID = 0
            };

            using (var content = new StringContent(JsonConvert.SerializeObject(seller)))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res = HttpClient.PostAsync(isMA ? "Create/MA" : "Create", content).Result;
                var x = res.Content.ReadAsStringAsync().Result;
                QRCodeSeller resBody = JsonConvert.DeserializeObject<QRCodeSeller>(x);
                using (var setCommissionRateContent = new StringContent(JsonConvert.SerializeObject(new { Rate = rate, resBody.SellerID })))
                {
                    setCommissionRateContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var s = await HttpClient.PostAsync("SetCommissionRate", setCommissionRateContent))
                    {
                        return resBody.SellerID;
                    }
                }
            }

        }

        /// <summary>
        /// 获取当前MA管理的全部分销ID
        /// </summary>
        /// <exception cref="UnExceptedException"/>
        /// <returns></returns>
        internal static QRCodeSeller[] GetSellerList()
        {
            if (JupiterClient.IsMA == false)
            {
                throw new UnExceptedException("权限不足，当前用户没有管理者权限");
            }

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JupiterClient.Token);
            var res = HttpClient.GetAsync("List").Result;
            var x = res.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<QRCodeSeller[]>(x);
        }

        internal static double GetTotalCommission()
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JupiterClient.Token);
            var res = HttpClient.GetAsync("Calculator").Result;
            var x = res.Content.ReadAsStringAsync().Result;
            return double.Parse(x);
        }

        /// <summary>
        /// 获取当前用户的分销id
        /// </summary>
        /// <returns></returns>
        internal static QRCodeSeller GetCurrentUserSellerInformation()
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JupiterClient.Token);
            var res = HttpClient.GetStringAsync("QRCodeSeller").Result;
            return JsonConvert.DeserializeObject<QRCodeSeller[]>(res)[0];
        }

        internal static double GetCurrentUserSellerRate()
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JupiterClient.Token);
            var res = HttpClient.GetStringAsync("Rate").Result;
            return JsonConvert.DeserializeObject<double>(res);
        }

        /// <summary>
        /// 更新分销ID的信息
        /// </summary>
        /// <param name="qRCodeSeller"></param>
        /// <returns></returns>
        internal static async void UpdateSellerInformation(QRCodeSeller seller)
        {
            if (JupiterClient.IsMA == false)
            {
                throw new UnExceptedException("权限不足，当前用户没有管理者权限");
            }
            using (var content = new StringContent(JsonConvert.SerializeObject(seller)))
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JupiterClient.Token);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var x = await HttpClient.PostAsync("Update", content);
                x.Dispose();
                //var c = await x.Content.ReadAsStringAsync();
            }

        }

        /// <summary>
        /// 指定分销ID，查询关联订单列表
        /// </summary>
        /// <param name="sellerID"></param>
        /// <returns></returns>
        internal static LitemallOrder[] GetOrderListBySellerID(int? sellerID = null)
        {
            if (sellerID != null && JupiterClient.IsMA == false)
            {
                throw new UnExceptedException("权限不足，当前用户没有管理者权限");
            }

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JupiterClient.Token);
            var res = HttpClient.GetAsync(sellerID == null ? "Orders" : $"Orders/{sellerID}").Result;
            var x = res.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<LitemallOrder[]>(x);
        }
    }
}
