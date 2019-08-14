using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class AdministratorDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
