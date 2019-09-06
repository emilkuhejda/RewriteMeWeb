using System;
using System.Runtime.Serialization;

namespace RewriteMe.Domain.Exceptions
{
    public class LanguageVersionNotExistsException : Exception
    {
        public LanguageVersionNotExistsException()
        {
        }

        public LanguageVersionNotExistsException(string message)
            : base(message)
        {
        }

        public LanguageVersionNotExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected LanguageVersionNotExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
