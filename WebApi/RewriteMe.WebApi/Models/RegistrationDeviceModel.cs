using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.WebApi.Models
{
    public class RegistrationDeviceModel
    {
        public Guid InstallationId { get; set; }

        public RuntimePlatform RuntimePlatform { get; set; }

        public string InstalledVersionNumber { get; set; }

        public Language Language { get; set; }
    }
}
