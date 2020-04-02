using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeManager.Authorizer
{
    public class UnauthorizeException:Exception
    {
        public UnauthorizeException() : base("身份认证失败")
        {

        }

        public UnauthorizeException(string message) : base(message)
        {

        }

    }
}
