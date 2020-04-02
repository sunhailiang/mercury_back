using System.Security.Cryptography;
using System.Text;

namespace JupiterClient
{
    public static class Calculator
    {
        public static string Md5(string input)
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
    }

}
