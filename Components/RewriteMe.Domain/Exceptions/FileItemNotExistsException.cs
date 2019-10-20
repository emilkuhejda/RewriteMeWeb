using System;
using System.Runtime.Serialization;

namespace RewriteMe.Domain.Exceptions
{
    public class FileItemNotExistsException : Exception
    {
        public FileItemNotExistsException()
        {
        }

        public FileItemNotExistsException(string message)
            : base(message)
        {
        }

        public FileItemNotExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FileItemNotExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
