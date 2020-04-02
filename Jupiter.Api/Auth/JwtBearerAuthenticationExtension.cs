/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Jupiter.Api.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public static class JwtBearerAuthenticationExtension
    {
        /// <summary>
        /// 注册JWT Bearer认证服务的静态扩展方法
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSettings">JWT授权的配置项</param>
        public static void AddJwtBearerAuthentication(this IServiceCollection services, AppAuthenticationSettings appSettings)
        {
            RSAParameters keyParams;
            while (!TryGetKeyParameters(appSettings, false, out keyParams))
            {
                GenerateAndSaveKey(appSettings);
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
                    ValidateAudience = false
                };
            });
        }

        public static string GetJwtAccessToken(AppAuthenticationSettings appSettings, ClaimsIdentity claimsIdentity)
        {
            RSAParameters keyParams;
            while (!TryGetKeyParameters(appSettings, true, out keyParams))
            {
                GenerateAndSaveKey(appSettings);
            }
            var key = new RsaSecurityKey(keyParams);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Jupiter",
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static IApplicationBuilder UseTokenUpdater(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                if (context.User.Identity.IsAuthenticated)
                {
#warning 在这里更新token
                }
                await next();
            });
        }

        /// <summary>
        /// 从本地文件中读取用来签发 Token 的 RSA Key
        /// </summary>
        /// <returns></returns>
        public static bool TryGetKeyParameters(AppAuthenticationSettings appSettings, bool withPrivate, out RSAParameters keyParameters)
        {
            keyParameters = default;
            string jsonString = withPrivate ? appSettings.PrivateKey : appSettings.PublicKey;
            try
            {
                keyParameters = JsonConvert.DeserializeObject<RSAParameters>(jsonString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            //string filename = withPrivate ? "key.json" : "key.public.json";
            //keyParameters = default(RSAParameters);
            //if (Directory.Exists(@"C:\Users\15716\Desktop") == false) return false;
            //keyParameters = JsonConvert.DeserializeObject<RSAParameters>(File.ReadAllText(Path.Combine(@"C:\Users\15716\Desktop", filename)));
            //return true;
        }

        /// <summary>
        /// 生成并保存 RSA 公钥与私钥
        /// </summary>
        public static RSAParameters GenerateAndSaveKey(AppAuthenticationSettings appSettings)
        {
            RSAParameters publicKeys, privateKeys;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    privateKeys = rsa.ExportParameters(true);
                    publicKeys = rsa.ExportParameters(false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            appSettings.PrivateKey = JsonConvert.SerializeObject(privateKeys);
            appSettings.PublicKey = JsonConvert.SerializeObject(publicKeys);
            return privateKeys;
        }
    }
}
