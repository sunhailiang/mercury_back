using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Api.Utils
{
    internal static class PasswordCalculator
    {
        /// <summary>
        /// 用户输入的密码通过此方法转换为MD5值
        /// </summary>
        /// <param name="input">用户输入</param>
        /// <returns></returns>
        internal static string Md5(string input)
        {
            byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < bytes.Length; i++)
            {
                sBuilder.Append(bytes[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        internal static string SaltPassword(string salt, string passwordMD5)
        {
            var input = $"{salt}{passwordMD5}Jupiter";
            return Md5(input);
        }
    }
}
