using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Models
{
    public class RegistrationUserModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string GivenName { get; set; }

        [Required]
        public string FamilyName { get; set; }

        public RegistrationDeviceModel Device { get; set; }
    }
}
