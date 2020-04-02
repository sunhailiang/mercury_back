using Newtonsoft.Json;
using SellerCodeManagerWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SellerCodeManagerWPF.Controllers
{
    internal class JupiterClient
    {
        private static readonly HttpClient httpClient;

        /// <summary>
        /// 令牌
        /// </summary>
        public static string Token { get; private set; }

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public static UserInformation UserInformation { get; private set; }

        /// <summary>
        /// 指示当前用户是否拥有QRCodeSellerCA权限码
        /// </summary>
        public static bool IsCA { get; private set; }

        /// <summary>
        /// 指示当前用户是否拥有QRCodeSellerMA权限码
        /// </summary>
        public static bool IsMA { get; private set; }

        /// <summary>
        /// 指示当前用户是否拥有QRCodeSeller权限码
        /// </summary>
        public static bool IsUser { get; private set; }

        internal static string Md5(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                foreach (byte v in bytes)
                {
                    sBuilder.Append(v.ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        static JupiterClient()
        {
            httpClient = new HttpClient { BaseAddress = new Uri("https://jupiter.clarabeautynism.com/") };
        }

        private async Task<UserInformation> GetProfile()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            using (var x = await httpClient.GetAsync("Account/Profile"))
            {
                var result = JsonConvert.DeserializeObject<JupiterMessage<UserInformation>>(await x.Content.ReadAsStringAsync());
                return result.Data;
            }
        }

        public async Task<JupiterMessage<string>> SignIn(string username, string password)
        {
            try
            {
                var passwordMD5 = Md5(password);
                //创建一个定时器，它将在一分钟后，之后每隔两分钟更新一次Token
                var result = await Login(passwordMD5);
                if (result.Code == 200 && !string.IsNullOrWhiteSpace(result.Data))
                {
                    Timer timer = new Timer(sender => Login(passwordMD5).Wait(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2));
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }

            async Task<JupiterMessage<string>> Login(string passwordMD5)
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(new { username, passwordMD5 })))
                {
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    using (var x = await httpClient.PostAsync("Account/SignIn", content))
                    {
                        var result = JsonConvert.DeserializeObject<JupiterMessage<string>>(await x.Content.ReadAsStringAsync());
                        if (result.Code == 200 && !string.IsNullOrWhiteSpace(result.Data))
                        {
                            Token = result.Data;
                            UserInformation = await GetProfile();
                            var permissions = (from role in UserInformation.Permissions
                                               from permission in role.Value
                                               select permission).Distinct();
                            IsCA = permissions.FirstOrDefault(y => y == "QRCodeSellerCA") != default;
                            IsMA = permissions.FirstOrDefault(y => y == "QRCodeSellerMA") != default;
                            IsUser = permissions.FirstOrDefault(y => y == "QRCodeSeller") != default;
                        }
                        return result;
                    }
                }
            }
        }

        public async Task<JupiterMessage<object>> ChangePassword(string oldPasswordMD5, string newPasswordMD5)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(new { OldPasswordMD5 = oldPasswordMD5, NewPasswordMD5 = newPasswordMD5 })))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JupiterClient.Token);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                //content.Headers.Add("Authorization", "Bearer " + Token);
                using (var x = await httpClient.PostAsync("Account/ChangePassword", content))
                {
                    var result = JsonConvert.DeserializeObject<JupiterMessage<object>>(await x.Content.ReadAsStringAsync());
                    return result;
                }
            }
        }

        public class JupiterMessage<T>
        {
            [JsonProperty(PropertyName = "code")]
            public int Code { get; set; }

            [JsonProperty(PropertyName = "message")]
            public string Message { get; set; }

            [JsonProperty(PropertyName = "data")]
            public T Data { get; set; }
        }
    }
}
