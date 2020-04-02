using AliyunSMS;
using JupiterClient;
using MercurySurvey.Models;
using MercurySurvey.Models.TraditionalMedicalConstitution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MercurySurvey.Controllers
{
    /// <summary>
    /// 该控制器用于处理中医辩证体质调查问卷，未来应当拆分至其他类库中
    /// </summary>
    [Route("[controller]/[action]")]
    public class TraditionalMedicalConstitutionController : Controller
    {
        private readonly NoteContext noteContext;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClient;

        /// <summary>
        /// 随机字符串，用于计算phoneInfoMd5
        /// </summary>
        private static readonly string randomString;

        static TraditionalMedicalConstitutionController()
        {
            randomString = Guid.NewGuid().ToString();//生产使用随机数
           //randomString = "B7863BA7-DE7E-427D-8A84-EBFFE1ED5317";//测试的时候使用
        }

        public TraditionalMedicalConstitutionController(NoteContext db, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            noteContext = db;
            configuration = config;
            httpClient = httpClientFactory;

        }

        ///// <summary>
        ///// 匿名用户通过该方法获取基本信息填写页面
        ///// </summary>
        ///// <returns>个人信息表单View</returns>
        ///// 未来移植到Jupiter用户注册页面
        //[HttpGet]
        //public IActionResult StartQuestionnaire()
        //{
        //    return View("StartQuestionnaireView");
        //}

        ///// <summary>
        ///// 匿名用户通过该方法提交个人信息表单 v0.5版本专用
        ///// </summary>
        ///// <param name="userInformation"></param>
        ///// <returns>Mercury流式问卷View</returns>
        ///// 未来移植到Jupiter用户注册页面
        ///// 该方法将自动在Jupiter注册同手机号用户
        ///// 该方法将通过JupiterClient获取该用户的BearerToken
        ///// 该方法将使用Razor将BearerToken渲染至返回的页面中
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public IActionResult SubmitUserInformation([FromForm]UserInformation userInformation)
        //{
        //    string[] phoneInfo = userInformation.PhoneInfo.Split("$");
        //    if (DateTime.Parse(phoneInfo[0]).AddMinutes(5) > DateTime.Now)
        //    {
        //        string phonecodePrimaryString = $"{userInformation.PhoneNumber}{phoneInfo[0]}{userInformation.PhoneCode}{randomString}";

        //        //判断Code是否正确
        //        if (Calculator.Md5(phonecodePrimaryString) == phoneInfo[1])
        //        {
        //            using (HttpClient client = httpClient.CreateClient())
        //            {
        //                string jupiterPath = configuration.GetSection("AppSetting").Get<JupiterKeys>().JupiterPath;

        //                StringContent content = new StringContent(JsonConvert.SerializeObject(new { userInformation.PhoneNumber, WeChatUnionID = default(string), Permission = default(string) }));
        //                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        //                Task<HttpResponseMessage> x = client.PostAsync($"{jupiterPath}/api/v1/User/AutoCreate?autoPassword=false&returnToken=true", content);
        //                JupiterMessage<AutoCreateResult> autoCreateResult = JsonConvert.DeserializeObject<JupiterMessage<AutoCreateResult>>(x.Result.Content.ReadAsStringAsync().Result);
        //                userInformation.UserGuid = autoCreateResult.Data.Guid;
        //                noteContext.UserInformation.Add(userInformation);
        //                noteContext.SaveChangesAsync();
        //                content.Dispose();

        //                //在此返回带token的答卷页面
        //                EntryPointViewModel vm = new EntryPointViewModel
        //                {
        //                    Guid = Guid.Parse("b546b709-2b2b-4f6e-9f1f-64f281de8d5b"),
        //                    Authorization = autoCreateResult.Data.Authorization
        //                };
        //                return View("FlexQuestionnaireView", vm);
        //            }
        //        }
        //    }
        //    return BadRequest("验证码错误或过期");
        //}

        /// <summary>
        /// 匿名用户通过该方法提交个人信息表单 v1.0版本专用
        /// </summary>
        /// <param name="userInformation"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult FastRegistration([FromForm]UserInformation userInformation)
        {
            //#warning 此处逻辑尚未完成
            string[] phoneInfo = userInformation.PhoneInfo.Split("$");
            if (DateTime.Parse(phoneInfo[0]).AddMinutes(5) > DateTime.Now)//生产的时候修改为5
            {
                string phonecodePrimaryString = $"{userInformation.PhoneNumber}{phoneInfo[0]}{userInformation.PhoneCode}{randomString}";
                //判断Code是否正确
                if (Calculator.Md5(phonecodePrimaryString) == phoneInfo[1])
                {
                    using (HttpClient client = httpClient.CreateClient())
                    {
                        //从JupiterApi内部接口自动登录，获取Token
                        string jupiterPath = configuration.GetSection("AppSetting").Get<JupiterKeys>().JupiterPath;
                        JupiterMessage<AutoCreateResult> autoCreateResult;
                        using (StringContent content = new StringContent(JsonConvert.SerializeObject(new { userInformation.PhoneNumber, WeChatUnionID = default(string), Permission = default(string) })))
                        {
                            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                            Task<HttpResponseMessage> x = client.PostAsync($"{jupiterPath}/api/v1/User/AutoCreate?autoPassword=false&returnToken=true", content);
                            autoCreateResult = JsonConvert.DeserializeObject<JupiterMessage<AutoCreateResult>>(x.Result.Content.ReadAsStringAsync().Result);
                            userInformation.UserGuid = autoCreateResult.Data.Guid;
                            noteContext.UserInformation.Add(userInformation);
                            //noteContext.SaveChangesAsync();//临时取消
                            noteContext.SaveChanges();
                        }
                        Response.Headers["Authorization"] = autoCreateResult.Data.Authorization;
                        var ticketID = QuestionnaireController.CreateTicket(Guid.Parse("b546b709-2b2b-4f6e-9f1f-64f281de8d5b"), autoCreateResult.Data.Guid, autoCreateResult.Data.Authorization);
                        return Ok(new { TicketID = ticketID });
                    }
                    //var ticketID = QuestionnaireController.CreateTicket(Guid.Parse("b546b709-2b2b-4f6e-9f1f-64f281de8d5b"), Guid.NewGuid(), "测试专用ticket");
                    //return Ok(new { TicketID = ticketID });
                }
                else
                {
                    return Forbid("验证码不正确");
                }
            }
            else
            {
                return Forbid("验证码已过期");
            }
        }

        /// <summary>
        /// 发送验证码请求
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult GetPhoneCode([FromForm]string phoneNumber)
        {
            string currentTime = DateTime.Now.ToString();
            Random random = new Random();
            int code = random.Next(1000, 9999);
            AliyunSms.SendSms(phoneNumber, "SMS_174992183", new { code }, "诫糖");
            string phonecodePrimaryString = $"{phoneNumber}{currentTime}{code}{randomString}";
            string phoneInfo = $"{currentTime}${Calculator.Md5(phonecodePrimaryString)}";
            //#warning 生产环境请删除此行！
            //Console.WriteLine($"验证码为：{code}");
            return Ok(new { phoneInfo });
        }

        ///// <summary>
        ///// 通过用户Guid查询该用户辩证体质测试结果，如果用户未能完成全部问题，将返回错误
        ///// </summary>
        ///// <param name="userGuid"></param>
        ///// <returns></returns>
        //[HttpGet("{userGuid}")]
        //public IActionResult Result1(Guid userGuid)
        //{
        //    //计分板
        //    Dictionary<string, double> scoreBoard = new Dictionary<string, double>
        //    {
        //        {"阴虚质",0d },
        //        {"阳虚质",0d },
        //        {"气虚质",0d },
        //        {"气郁质",0d },
        //        {"血瘀质",0d },
        //        {"湿热质",0d },
        //        {"痰湿质",0d },
        //        {"特禀质",0d },
        //        {"平和质",0d }
        //    };
        //    //拉取用户所有答案
        //    IQueryable<Answer> userAnswer = from answer in noteContext.Answer
        //                                    where answer.Userid == userGuid
        //                                    where answer.Mtime != DateTime.MinValue
        //                                    //where questionGuids.Contains(answer.QuestionGuid)
        //                                    select answer;
        //    //遍历问卷问题数据，在用户答案中查找匹配值
        //    foreach (Guid question in questionGuids)
        //    {
        //        try
        //        {
        //            scoreBoard[questionConstitutions.First(x => x.QuestionGuid == question).Constitution] += GetScore(question, userAnswer.First(x => x.QuestionGuid == question).Content);
        //        }
        //        catch (InvalidOperationException ex) when (ex.Message == "Sequence contains no elements")
        //        {
        //            //两道性别相关的问题用户必然缺失一道，捕获异常
        //            if (question == Guid.Parse("23E317C9-D691-402E-8249-C0E4D3ED78D0") || question == Guid.Parse("A6C1DBC2-233D-4FA8-927B-8BAED0F709B3"))
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                //如果用户未能答完问卷，将返回此错误
        //                throw;
        //            }
        //        }
        //    }

        //    //结果列表
        //    List<Regime> result = new List<Regime>();
        //    //表现为平和质
        //    bool mayPingheCons = true;
        //    //倾向于平和质
        //    bool tendPingheCons = true;
        //    //遍历计分板的九个key
        //    foreach (KeyValuePair<string, double> score in scoreBoard)
        //    {
        //        //平和质计算需要其他体质的计算结果，先跳过
        //        if (score.Key == "平和质")
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            int questionCount;
        //            switch (score.Key)
        //            {
        //                case "湿热质":
        //                    questionCount = 6;
        //                    break;
        //                case "阳虚质":
        //                case "血瘀质":
        //                case "气郁质":
        //                case "特禀质":
        //                    questionCount = 7;
        //                    break;
        //                case "阴虚质":
        //                case "气虚质":
        //                case "痰湿质":
        //                    questionCount = 8;
        //                    break;
        //                default:
        //                    throw new InvalidOperationException("体质定义错误");
        //            }
        //            double transScore = (score.Value - questionCount) * 100 / questionCount / 4;
        //            if (transScore >= 40)
        //            {
        //                result.Add(new Regime { Title = score.Key, CurrentCent = transScore, StandCent = 40 });
        //                //当体质偏颇时，不符合平和质标准，置false
        //                mayPingheCons = false;
        //                tendPingheCons = false;
        //            }
        //            else if (transScore <= 39 && transScore >= 30)
        //            {
        //                result.Add(new Regime { Title = $"倾向{score.Key}", CurrentCent = transScore, StandCent = 30 });
        //                mayPingheCons = false;
        //            }

        //        }
        //    }
        //    //如果没有任何体质偏颇，检查是否符合平和质标准
        //    if (tendPingheCons)
        //    {
        //        double transScore = (scoreBoard["平和质"] - 8) * 25 / 8;
        //        if (transScore >= 60)
        //        {
        //            result.Add(new Regime { Title = mayPingheCons ? "平和质" : "倾向平和质", CurrentCent = transScore, StandCent = 30 });
        //        }
        //    }
        //    //Razor渲染结果页
        //    //return View("ResultPageView", result);
        //    return Json(result);
        //}

        /// <summary>
        /// 通过用户Guid查询该用户辩证体质测试结果，如果用户未能完成全部问题，将返回错误
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet("{userGuid}")]
        public IActionResult Result(Guid userGuid)
        {

            //计分板
            Dictionary<string, double> scoreBoard = new Dictionary<string, double>
            {
                {"阴虚质",0d },
                {"阳虚质",0d },
                {"气虚质",0d },
                {"气郁质",0d },
                {"血瘀质",0d },
                {"湿热质",0d },
                {"痰湿质",0d },
                {"特禀质",0d },
                {"平和质",0d }
            };
            //拉取用户所有答案
            IQueryable<Answer> userAnswer = from answer in noteContext.Answer
                                            where answer.Userid == userGuid
                                            where answer.Mtime != DateTime.MinValue
                                            where answer.Ctime.Date==DateTime.Now.Date//兼容多次测试
                                            //where questionGuids.Contains(answer.QuestionGuid)
                                            select answer;
            //遍历问卷问题数据，在用户答案中查找匹配值
            foreach (Guid question in questionGuids)
            {
                try
                {
                    scoreBoard[questionConstitutions.First(x => x.QuestionGuid == question).Constitution] += GetScore(question, userAnswer.First(x => x.QuestionGuid == question).Content);
                }
                catch (InvalidOperationException ex) when (ex.Message == "Sequence contains no elements")
                {
                    //两道性别相关的问题用户必然缺失一道，捕获异常
                    if (question == Guid.Parse("23E317C9-D691-402E-8249-C0E4D3ED78D0") || question == Guid.Parse("A6C1DBC2-233D-4FA8-927B-8BAED0F709B3"))
                    {
                        continue;
                    }
                    else
                    {
                        //如果用户未能答完问卷，将返回此错误
                        throw;
                    }
                }
            }

            //结果列表
            List<Regime> result = new List<Regime>();
            //表现为平和质
            bool mayPingheCons = true;
            //倾向于平和质
            bool tendPingheCons = true;
            var username = (from name in noteContext.UserInformation
                            where name.UserGuid == userGuid
                            select name).FirstOrDefault();
            //遍历计分板的九个key
            foreach (KeyValuePair<string, double> score in scoreBoard)
            {
                //平和质计算需要其他体质的计算结果，先跳过
                if (score.Key == "平和质")
                {
                    continue;
                }
                else
                {
                    int questionCount;
                    switch (score.Key)
                    {
                        case "湿热质":
                            questionCount = 6;
                            break;
                        case "阳虚质":
                        case "血瘀质":
                        case "气郁质":
                        case "特禀质":
                            questionCount = 7;
                            break;
                        case "阴虚质":
                        case "气虚质":
                        case "痰湿质":
                            questionCount = 8;
                            break;
                        default:
                            throw new InvalidOperationException("体质定义错误");
                    }
                    double transScore = (score.Value - questionCount) * 100 / questionCount / 4;
                    if (transScore >= 40)
                    {
                        result.Add(new Regime { Title = score.Key, CurrentCent = transScore, StandCent = 40, UserName = username.UserName });
                        //当体质偏颇时，不符合平和质标准，置false
                        mayPingheCons = false;
                        tendPingheCons = false;
                    }
                    else if (transScore <= 39 && transScore >= 30)
                    {
                        result.Add(new Regime { Title = $"倾向{score.Key}", CurrentCent = transScore, StandCent = 30, UserName = username.UserName });
                        mayPingheCons = false;
                    }

                }
            }
            //如果没有任何体质偏颇，检查是否符合平和质标准
            if (tendPingheCons)
            {
                double transScore = (scoreBoard["平和质"] - 8) * 25 / 8;
                if (transScore >= 60)
                {
                    result.Add(new Regime { Title = mayPingheCons ? "平和质" : "倾向平和质", CurrentCent = transScore, StandCent = 60, UserName = username.UserName });
                }
                else
                {
                    result.Add(new Regime { Title = "平和质", CurrentCent = transScore, StandCent = 0, UserName = username.UserName });
                }
            }
            return Json(result);
        }

        #region 移植源码
        /// <summary>
        /// 问题GUID-辩证体质关系对
        /// </summary>
        private class QuestionConstitution
        {
#warning 把Regime.Constitution改成string[]类型后，此处应同步传入Constitution字段，或string[].Index
            public string Constitution { get; set; }

            public Guid QuestionGuid { get; set; }
        }

        /// <summary>
        /// 获取问题Guid和体质的对应关系
        /// </summary>
        /// <returns>指示每道问题属于哪一种体质</returns>
        private static QuestionConstitution[] GetConstitutionPairList()
        {
            //创建数组，储存每道问题属于哪一种体质
            string jsonString = "[{\"Constitution\":\"平和质\",\"QuestionGuid\":\"ae4b39ed-501e-4654-a16c-45ce98fc53e2\"},{\"Constitution\":\"平和质\",\"QuestionGuid\":\"06bb9ddc-1725-4900-8ea8-9c2f1f2d24ca\"},{\"Constitution\":\"平和质\",\"QuestionGuid\":\"5ea1223c-9497-4530-a0c2-a66bea9dbd2f\"},{\"Constitution\":\"平和质\",\"QuestionGuid\":\"7b17ef28-bf0e-4635-ad1d-b68f22dd60a6\"},{\"Constitution\":\"平和质\",\"QuestionGuid\":\"c6dd0bee-f6f5-46f6-8cdb-e278ff6ba5c7\"},{\"Constitution\":\"平和质\",\"QuestionGuid\":\"966ae059-f094-4f7b-8045-eaa05e539944\"},{\"Constitution\":\"平和质\",\"QuestionGuid\":\"01220615-766b-4ded-9097-eb15f0287507\"},{\"Constitution\":\"平和质\",\"QuestionGuid\":\"48fa4d9e-01a1-44d6-9226-efe6ea4a2dbe\"},{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"f721faf1-602e-4167-82f0-0b0fc7ca5974\"},{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"1ae0eac1-8b9c-4d13-bf3b-134f0f1ab60c\"},{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"be14714a-d174-4180-a330-1c601d6e79b9\"},{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"a857027c-71ea-48ef-b991-23104db9dc4c\"},{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"8f4d90f2-055f-4d99-8c2e-4c115e290d5b\"},{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"3d1df631-aad5-4786-8f46-76c768f42133\"},{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"66c27b32-e168-404c-b20e-7bf5c895c143\"},{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"86cd1cff-5029-4ce7-90bf-cab9710a7e5d\"},{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"02f5d0f4-a4d8-41d1-8ee2-08d8f5778d2b\"},{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"683a60a8-ced6-480c-89f7-1dcf7eac829d\"},{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"db72a506-edd9-4d7f-981b-57b6c36e0fcd\"},{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"c58bbc67-131c-4872-a610-6679f9fa4cf9\"},{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"bdb48675-8933-4e74-a824-9442bf11a600\"},{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"d01ea475-3cc8-4c5b-966f-a8bd762a1081\"},{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"8eafa69f-8fb9-411f-9f9b-cea3391db8c3\"},{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"604045a2-92c3-4da8-a675-46a162d68d9d\"},{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"9df2ecc2-07dc-4253-b7c4-504515396dd2\"},{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"a6c1dbc2-233d-4fa8-927b-8baed0f709b3\"},{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"7ca4bddd-8b36-4454-8513-bec3100ec2a0\"},{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"23e317c9-d691-402e-8249-c0e4d3ed78d0\"},{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"5d2abf28-71c5-470d-959a-c75237f0689e\"},{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"856a18ee-9d37-4348-9a1a-ff3fca7ca2e4\"},{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"f6180ada-9871-45c4-aadd-28ed8b1b1841\"},{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"4eff98fe-1957-44b5-b084-37bf67e5356c\"},{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"23982696-2722-4dc3-a1e2-524d72a38197\"},{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"b1a5da63-a4fc-4505-b71e-86dacbcec6f8\"},{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"6d18ad7c-2781-4b73-ad9b-9186b2e87fde\"},{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"16b8dbc7-4d7f-42c1-90c2-aae5e8870549\"},{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"824b8f3c-7df5-4ed0-8473-ba167a94dcec\"},{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"068e7f5b-a70e-46ab-8186-e1ed89e20a96\"},{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"b26af282-ed74-45cc-95ed-03e55739d0d9\"},{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"cacf9241-580e-485b-8c39-4cd474319d11\"},{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"7744a6a9-8dc8-4491-b62a-58b0bbb97468\"},{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"8f6ea2be-e7a1-4120-a016-5e8b50c6f5f2\"},{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"f74f9607-73aa-468b-90a4-6e0f435b7455\"},{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"99c780f6-b005-4d40-9660-6e565baf556c\"},{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"d15cf76c-c8b6-46d8-9ce5-c2754ee33db9\"},{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"0324dd99-8371-42c4-a6ef-0c558c88a40b\"},{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"0b4231ed-1340-4a57-832f-38c0f8b3d795\"},{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"4669abf8-d1f7-42e7-a96b-3df34da8dbc0\"},{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"3b8c85d9-a8df-404e-8410-4669817d8139\"},{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"c05d4411-3043-4d72-bd28-ecdca6a6a1a1\"},{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"393cd5f8-1bcf-434f-9208-f64a497622af\"},{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"dff885fd-df8c-4075-9484-fc3e9afe8e7d\"},{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"e1f6f091-3d1a-42c0-abf6-07babde77e93\"},{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"d9228fcf-5467-49ce-861f-67e603b8e5c0\"},{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"78bc2059-ade0-4478-93ab-9314e8d817c3\"},{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"e585e827-3b70-4b8c-9c86-9f7fe59df6d0\"},{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"6a51c98d-2900-4cd5-8eab-a20b405533ca\"},{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"4460bf86-7caa-4180-bd8b-e19f04596adc\"},{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"29dfedf4-4ddf-4a23-99f3-fc399e15df1e\"},{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"79b98ed2-58e7-465f-9985-12041dfb6332\"},{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"25801e21-5e5d-4b70-b688-30b273ee8b23\"},{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"c0290174-8f90-405d-86c3-5de55a814a7c\"},{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"dff9548d-d0f1-4c62-80ec-6b1444c76ee0\"},{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"fdf47022-406f-470c-860f-711a03c6cf4f\"},{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"1433f352-b2d6-41ab-8c59-75bbdb9a8a53\"},{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"6b354226-1925-4d0e-959c-cc2dc731e9cd\"},{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"f2effb5f-b75f-4be4-bcb0-eac877a8f344\"}]";
            return JsonConvert.DeserializeObject<QuestionConstitution[]>(jsonString);
        }

        /// <summary>
        /// 记录每一道问题属于哪种体质的数组
        /// </summary>
        private static readonly QuestionConstitution[] questionConstitutions = GetConstitutionPairList();

        /// <summary>
        /// 记录本问卷的问题列表
        /// </summary>
        private static readonly Guid[] questionGuids = (from questionConstitution in questionConstitutions select questionConstitution.QuestionGuid).ToArray();

        /// <summary>
        /// 传入问题Guid和用户回答JsonString，计算分数
        /// </summary>
        /// <param name="questionGuid"></param>
        /// <param name="answerContent"></param>
        /// <returns>分数</returns>
        private double GetScore(Guid questionGuid, string answerContent)
        {
#warning 把Constitution改成string[]即可解决同一个问题隶属于多个体质时题目重复的问题

            //SqlServer中储值为JsonString，反序列化
            string content = JsonConvert.DeserializeObject<string>(answerContent);
            //偏颇体质的问题计分方式均为Common
            if (questionConstitutions.First(x => x.QuestionGuid == questionGuid).Constitution != "平和质")
            {
                return Common();
            }
            //平和质中有两道题的计分方式为Common
            else if (questionGuid == Guid.Parse("ae4b39ed-501e-4654-a16c-45ce98fc53e2") || questionGuid == Guid.Parse("48fa4d9e-01a1-44d6-9226-efe6ea4a2dbe"))
            {
                return Common();






            }
            //平和质中有一些题的计分方式是Special
            else
            {
                return Special();
            }

            //内联函数，顺序的加分方式
            double Common()
            {
                switch (content)
                {
                    case "没有(根本不)": return 1d;
                    case "很少(有一点)": return 2d;
                    case "有时(有些)": return 3d;
                    case "经常(相当)": return 4d;
                    case "总是(非常)": return 5d;
                    default: return 0d;
                }
            }

            //内联函数，反序的加分方式
            double Special()
            {
                switch (content)
                {
                    case "没有(根本不)": return 5d;
                    case "很少(有一点)": return 4d;
                    case "有时(有些)": return 3d;
                    case "经常(相当)": return 2d;
                    case "总是(非常)": return 1d;
                    default: return 0d;
                }
            }
        }
        #endregion

        /// <summary>
        /// 输入userid查询出经常(相当)和总是(非常)
        /// </summary>
        /// <param name="userguid">用户的guid</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserAnswer(Guid userguid)
        {
            List<AnswerQuestion> answerQuestions = new List<AnswerQuestion>();
            Answer[] answers = (from answer in noteContext.Answer
                                where answer.Userid == userguid && answer.Content == JsonConvert.SerializeObject("经常(相当)")
                                || answer.Content == JsonConvert.SerializeObject("总是(非常)")
                                select answer).ToArray();
            foreach (Answer item in answers)
            {
                Question[] result = (from question in noteContext.Question
                                     where item.QuestionVersion == question.Version
                                     select question).ToArray();
                answerQuestions.Add(new AnswerQuestion { AnswerContent = JsonConvert.DeserializeObject<string>(item.Content), QuestionContent = JsonConvert.DeserializeObject(result.First().Content) });
            }
            return Json(answerQuestions);
        }


        #region 微信接口的调用  

        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        private string wxAccess_token()
        {
            string AppId = "wxa76dfc44f4141c4c";
            string AppSecret = "e23771971d294d4403ece898138cc01f";
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}" + AppId + AppSecret;
            var httpClient = new HttpClient();
            var access = httpClient.GetAsync(url);
            return access.ToString();

        }
        private static Access_token GetAccess_token()
        {
            string AppId = "wxa76dfc44f4141c4c";
            string AppSecret = "e23771971d294d4403ece898138cc01f";
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret;
            Access_token mode = new Access_token();

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);  //用GET形式请求指定的地址 
            req.Method = "GET";

            using (WebResponse wr = req.GetResponse())
            {
                //HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();  
                StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                //在这里对Access_token 赋值  
                Access_token token = new Access_token();
                token = JsonConvert.DeserializeObject<Access_token>(content);
                mode.access_token = token.access_token;
                mode.expires_in = token.expires_in;
            }
            return mode;
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <returns></returns>
        //public string wx_jsapi_ticket()
        //{
        //    Access_token access_tokens = GetAccess_token();
        //    string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=wx_card"+ access_tokens.access_token;
        //    HttpClient httpClient = new HttpClient();
        //    var ticket=httpClient.GetAsync(url);
        //    return ticket.ToString();
        //}
        private static wx_ticket GetTicket()
        {
            Access_token token = GetAccess_token();
            //string jsapi_ticket = "";//唯一凭证
            string jsurl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token.access_token + "&type=jsapi";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(jsurl);  //用GET形式请求指定的地址 
            req.Method = "GET";
            wx_ticket wx_ = new wx_ticket();
            using (WebResponse wr = req.GetResponse())
            {
                //HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();  
                StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                //jsapi_ticket = content;
                reader.Close();
                reader.Dispose();
                wx_ticket wx_Ticket = new wx_ticket();
                wx_Ticket = JsonConvert.DeserializeObject<wx_ticket>(content);
                wx_.ticket = wx_Ticket.ticket;
            }
            //WriteLogs("piaoju", "ticket", jsapi_ticket);
            return wx_;

        }

        /// <summary>
        /// 返回appid 时间戳 随机数 签名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getwxconfig(string url)
        {
            wx_ticket ticket = GetTicket();
            string nonce = CreateNonceStr();
            string timestamp = GetTimeStamp();
            //string wxconfigstr = "jsapi_ticket=" + ticket.ticket + "&noncestr=" + nonce + "&timestamp=" + timestamp + "&url=" + url + "";
            var string1Builder = new StringBuilder();
            //注意这里参数名必须全部小写，且必须有序
            string1Builder.Append("jsapi_ticket=").Append(ticket.ticket).Append("&")
                          .Append("noncestr=").Append(nonce).Append("&")
                          .Append("timestamp=").Append(timestamp).Append("&")
                          .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);

            //string[] arrtmp = {ticket.ticket,nonce,timestamp,url };
            //Array.Sort(arrtmp);//字典排序  
            //string tmpstr = string.Join("", arrtmp);

            //var buffer = Encoding.UTF8.GetBytes(string1Builder);
            //var data = SHA1.Create().ComputeHash(buffer);//sha1加密
            string tickets = Sha1(string1Builder.ToString(), Encoding.UTF8);
            wx_model wx_Model = new wx_model
            {
                appId = "wxa76dfc44f4141c4c",
                timestamp = GetTimeStamp(),
                nonce = CreateNonceStr(),
                signature = tickets,
            };
            return Json(wx_Model);
        }
        /// <summary>
        /// sha1加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string Sha1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytesIn = encode.GetBytes(content);
                byte[] bytesOut = sha1.ComputeHash(bytesIn);
                sha1.Dispose();
                string result = BitConverter.ToString(bytesOut);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// 生成随机数字字符串
        /// 待生成的位数
        /// 生成的数字字符串
        private int rep = 0;
        //private string GenerateCheckCodeNum(int codeCount)
        //{
        //    string str = string.Empty;
        //    long num2 = DateTime.Now.Ticks + this.rep;
        //    this.rep++;
        //    Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
        //    for (int i = 0; i < codeCount; i++)
        //    {
        //        int num = random.Next();
        //        str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
        //    }
        //    return str;
        //}
        /// <summary>
        /// 随机字符串数组集合
        /// </summary>
        private static readonly string[] NonceStrings = new string[]
                                    {
                                    "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                                    "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
                                    };

        /// <summary>
        /// 生成签名的随机串
        /// </summary>
        /// <returns></returns>
        private static string CreateNonceStr()
        {
            Random random = new Random();
            var sb = new StringBuilder();
            var length = NonceStrings.Length;

            //生成15位数的随机字符串，当然也可以通过控制对应字符串大小生成，但是至多不超过32位
            for (int i = 0; i < 15; i++)
            {
                sb.Append(NonceStrings[random.Next(length - 1)]);//通过random获得的随机索引到，NonceStrings数组中获取对应数组值
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取微信网页开发js.sdk所需要的数据
        /// </summary>
        public class wx_model
        {
            /// <summary>
            /// 微信公众号的appid这个是有微信服务器端签发
            /// </summary>
            public string appId { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            public string timestamp { get; set; }
            /// <summary>
            /// 随机字符串
            /// </summary>
            public string nonce { get; set; }
            /// <summary>
            /// 根据access_token生成ticketId再生成签名
            /// 签名内容包含 appid ticketid 时间戳 随机字符串
            /// </summary>
            public string signature { get; set; }
        }
        /// <summary>
        /// 获取微信签发的Access_token
        /// </summary>
        public class Access_token
        {
            /// <summary>
            /// 访问签发接口得到的access_token
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 访问签发接口得到的expires_in时间一般为7200两个小时有效期
            /// </summary>
            public string expires_in { get; set; }
        }
        /// <summary>
        /// 访问ticket id接口时返回的参数
        /// </summary>
        public class wx_ticket
        {
            /// <summary>
            /// int 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// string 错误信息
            /// </summary>
            public string errmsg { get; set; }
            /// <summary>
            /// string 临时票据用于在获取授权连接时作为参数传入
            /// </summary>
            public string ticket { get; set; }
            /// <summary>
            /// int ticket的有效期一般为7200秒
            /// </summary>
            public int expires_in { get; set; }

        }


        #endregion


    }
}
