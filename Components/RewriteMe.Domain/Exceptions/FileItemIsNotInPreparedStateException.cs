using System;
using System.Runtime.Serialization;

namespace RewriteMe.Domain.Exceptions
{
    public class FileItemIsNotInPreparedStateException : Exception
    {
        public FileItemIsNotInPreparedStateException()
        {
        }

        public FileItemIsNotInPreparedStateException(string message)
            : base(message)
        {
        }

        public FileItemIsNotInPreparedStateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FileItemIsNotInPreparedStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
