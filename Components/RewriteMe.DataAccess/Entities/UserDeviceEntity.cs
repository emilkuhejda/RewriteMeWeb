using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.DataAccess.Entities
{
    public class UserDeviceEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid InstallationId { get; set; }

        public RuntimePlatform RuntimePlatform { get; set; }

        public string InstalledVersionNumber { get; set; }

        public Language Language { get; set; }

        public DateTime DateRegisteredUtc { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
