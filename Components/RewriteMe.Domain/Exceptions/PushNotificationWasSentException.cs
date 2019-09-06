using System;
using System.Runtime.Serialization;

namespace RewriteMe.Domain.Exceptions
{
    public class PushNotificationWasSentException : Exception
    {
        public PushNotificationWasSentException()
        {
        }

        public PushNotificationWasSentException(string message)
            : base(message)
        {
        }

        public PushNotificationWasSentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PushNotificationWasSentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
