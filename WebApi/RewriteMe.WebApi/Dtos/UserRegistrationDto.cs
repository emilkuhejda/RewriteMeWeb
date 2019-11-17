using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class UserRegistrationDto
    {
        [Required]
        public string Token { get; set; }

        public IdentityDto Identity { get; set; }

        public TimeSpanWrapperDto RemainingTime { get; set; }
    }
}
