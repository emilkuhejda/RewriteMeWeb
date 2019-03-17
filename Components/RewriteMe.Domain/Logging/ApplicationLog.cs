using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Logging
{
    public class ApplicationLog
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public ApplicationLogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
