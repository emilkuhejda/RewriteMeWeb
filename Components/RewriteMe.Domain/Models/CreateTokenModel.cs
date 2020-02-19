using System;
using System.ComponentModel.DataAnnotations;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Models
{
    public class CreateTokenModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
