using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Notifications
{
    public class UserDevice
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid InstallationId { get; set; }

        public RuntimePlatform RuntimePlatform { get; set; }

        public string InstalledVersionNumber { get; set; }

        public Language Language { get; set; }

        public DateTime DateRegisteredUtc { get; set; }
    }
}
