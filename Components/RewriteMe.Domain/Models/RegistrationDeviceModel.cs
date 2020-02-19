using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Models
{
    public class RegistrationDeviceModel
    {
        [Required]
        public Guid InstallationId { get; set; }

        [Required]
        public string RuntimePlatform { get; set; }

        [Required]
        public string InstalledVersionNumber { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
