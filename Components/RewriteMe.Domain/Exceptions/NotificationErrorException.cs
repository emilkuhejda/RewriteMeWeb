using System;
using System.Runtime.Serialization;
using RewriteMe.Domain.Notifications;

namespace RewriteMe.Domain.Exceptions
{
    public class NotificationErrorException : Exception
    {
        public NotificationErrorException()
        {
        }

        public NotificationErrorException(NotificationError notificationError)
        {
            NotificationError = notificationError;
        }

        public NotificationErrorException(string message)
            : base(message)
        {
        }

        public NotificationErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotificationErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public NotificationError NotificationError { get; }
    }
}
