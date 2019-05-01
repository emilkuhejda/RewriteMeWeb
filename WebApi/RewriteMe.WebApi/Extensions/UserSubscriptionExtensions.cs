using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Extensions
{
    public static class UserSubscriptionExtensions
    {
        public static UserSubscriptionDto ToDto(this UserSubscription userSubscription)
        {
            return new UserSubscriptionDto
            {
                Id = userSubscription.Id,
                UserId = userSubscription.UserId,
                Time = userSubscription.Time,
                DateCreated = userSubscription.DateCreated
            };
        }
    }
}
