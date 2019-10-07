using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Models
{
    public class UpdateAdministratorModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
