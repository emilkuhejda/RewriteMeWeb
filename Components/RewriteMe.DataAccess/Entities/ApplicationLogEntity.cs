using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.DataAccess.Entities
{
    public class ApplicationLogEntity
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public ApplicationLogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public DateTime DateCreated { get; set; }

        public UserEntity User { get; set; }
    }
}
