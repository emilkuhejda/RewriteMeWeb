using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class RegistrationModelDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public UserSubscriptionDto UserSubscription { get; set; }
    }
}
