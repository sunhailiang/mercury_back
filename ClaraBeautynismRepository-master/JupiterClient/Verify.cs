using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JupiterClient
{
    public static class Verify
    {
        /// <summary>
        /// 向容器注入Jupiter Token检查服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            var appSettingSection = Configuration.GetSection("AppSetting");
            services.Configure<JupiterKeys>(appSettingSection);
            var keys = appSettingSection.Get<JupiterKeys>();

            if (!TryGetKeyParameters(keys, out RSAParameters keyParams))
            {
                throw new UnauthorizeException();
            }

            var key = new RsaSecurityKey(keyParams);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddCookie(cfg => cfg.SlidingExpiration = true)
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = "Jupiter",
                    ValidateAudience = false,
                };
            });

            services.AddHttpClient();

            return services;
        }


        /// <summary>
        /// 从本地文件中读取用来签发 Token 的 RSA Key
        /// </summary>
        /// <param name="filePath">存放密钥的文件夹路径</param>
        /// <param name="withPrivate"></param>
        /// <param name="keyParameters"></param>
        /// <returns></returns>
        private static bool TryGetKeyParameters(JupiterKeys keys, out RSAParameters keyParameters)
        {
            //keyParameters = default(RSAParameters);
            keyParameters = JsonConvert.DeserializeObject<RSAParameters>(keys.PublicKey);
            return true;
        }

        /// <summary>
        /// Token自动刷新中间件，请务必放在UseAuthentication后，UseMVC前
        /// </summary>
        /// <param name="app"></param>
        /// <param name="jupiterKeys"></param>
        /// <param name="http"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAutoRefreshToken(this IApplicationBuilder app, JupiterKeys jupiterKeys, IHttpClientFactory http)
        {
            app.Use(async (context, next) =>
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    string refreshToken = CheckTokenExp(context, jupiterKeys, http);
                    if (refreshToken != default)
                    {
                        context.Response.Headers["Authorization"] = refreshToken;
                    }
                }
                await next();
            });

            return app;

            string CheckTokenExp(HttpContext context, JupiterKeys keys, IHttpClientFactory httpClient)
            {
                var iat = long.Parse(context.User.Claims.First(x => x.Type == "iat").Value);
                //var now = DateTime.UtcNow.TotalSeconds
                var iatDateTime = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(iat);
                var now = DateTime.UtcNow;
                if ((now - iatDateTime) > TimeSpan.FromMinutes(5))
                {
                    try
                    {
                        return RefreshToken();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        return default;
                    }
                }
                else
                {
                    return default;
                }

                string RefreshToken()
                {
                    var token = context.Request.Headers["Authorization"][0];
                    using (var client = http.CreateClient())
                    {
                        client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
                        //Console.WriteLine($"{jupiterKeys.JupiterPath}/api/Oauth/RefreshToken");
                        var result = client.GetAsync($"{jupiterKeys.JupiterPath}/api/Oauth/RefreshToken");
                        JObject jo = JObject.Parse(result.Result.Content.ReadAsStringAsync().Result);
                        return jo["data"].ToString();
                    }
                }
            }
        }
    }

}
