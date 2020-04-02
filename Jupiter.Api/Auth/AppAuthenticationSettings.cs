/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

namespace Jupiter.Api.Auth
{
    /// <summary>
    /// JWT授权的配置项
    /// </summary>
    public class AppAuthenticationSettings
    {
        /// <summary>
        /// Json格式的JupiterIdentityServer RSA私钥
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Json格式的JupiterIdentityServer RSA公钥
        /// </summary>
        public string PublicKey { get; set; }
    }
}