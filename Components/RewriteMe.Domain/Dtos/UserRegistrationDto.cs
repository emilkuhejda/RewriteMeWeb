using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Dtos
{
    public class UserRegistrationDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        public IdentityDto Identity { get; set; }

        public TimeSpanWrapperDto RemainingTime { get; set; }
    }
}
