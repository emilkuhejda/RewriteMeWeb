using System;
using System.Runtime.Serialization;

namespace RewriteMe.Domain.Exceptions
{
    public class EmptyDeviceListException : Exception
    {
        public EmptyDeviceListException()
        {
        }

        public EmptyDeviceListException(string message)
            : base(message)
        {
        }

        public EmptyDeviceListException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EmptyDeviceListException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
