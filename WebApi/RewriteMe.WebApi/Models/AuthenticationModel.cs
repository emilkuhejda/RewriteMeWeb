using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Models
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
