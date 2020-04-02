using System;

namespace JupiterClient
{
    /// <summary>
    /// 身份认证失败异常类
    /// </summary>
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException() : base("身份认证失败")
        {

        }

        public UnauthorizeException(string message) : base(message)
        {

        }

    }

}
