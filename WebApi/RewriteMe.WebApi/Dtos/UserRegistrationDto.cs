using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class UserRegistrationDto
    {
        [Required]
        public string Token { get; set; }

        public UserDto Identity { get; set; }

        public UserSubscriptionDto UserSubscription { get; set; }
    }
}
