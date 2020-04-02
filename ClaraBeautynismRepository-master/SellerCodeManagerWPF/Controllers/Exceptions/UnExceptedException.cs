using System;
using System.Runtime.Serialization;

namespace SellerCodeManagerWPF.Controllers.Exceptions
{
    [Serializable]
    internal class UnExceptedException : Exception
    {
        public UnExceptedException()
        {
        }

        public UnExceptedException(string message) : base(message)
        {
        }

        public UnExceptedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnExceptedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}