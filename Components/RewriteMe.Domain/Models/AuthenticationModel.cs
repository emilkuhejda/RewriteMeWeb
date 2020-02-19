using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Models
{
    public class AuthenticationModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
