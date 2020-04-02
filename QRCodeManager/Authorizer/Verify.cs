using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QRCodeManager.ConfigModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace QRCodeManager.Authorizer
{
    public static class Verify
    {
        public static void AddJwtBearerAuthentication(this IServiceCollection services, JupiterKeys keys)
        {

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
        }


        /// <summary>
        /// 从本地文件中读取用来签发 Token 的 RSA Key
        /// </summary>
        /// <param name="filePath">存放密钥的文件夹路径</param>
        /// <param name="withPrivate"></param>
        /// <param name="keyParameters"></param>
        /// <returns></returns>
        public static bool TryGetKeyParameters(JupiterKeys keys, out RSAParameters keyParameters)
        {
            //keyParameters = default(RSAParameters);
            keyParameters = JsonConvert.DeserializeObject<RSAParameters>(keys.PublicKey);
            return true;
        }

    }
}
