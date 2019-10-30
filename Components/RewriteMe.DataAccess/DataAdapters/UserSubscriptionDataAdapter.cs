using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Settings;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class UserSubscriptionDataAdapter
    {
        public static UserSubscription ToUserSubscription(this UserSubscriptionEntity entity)
        {
            return new UserSubscription
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ApplicationId = entity.ApplicationId,
                Time = entity.Time,
                DateCreatedUtc = entity.DateCreatedUtc
            };
        }

        public static UserSubscriptionEntity ToUserSubscriptionEntity(this UserSubscription userSubscription)
        {
            return new UserSubscriptionEntity
            {
                Id = userSubscription.Id,
                UserId = userSubscription.UserId,
                ApplicationId = userSubscription.ApplicationId,
                Time = userSubscription.Time,
                DateCreatedUtc = userSubscription.DateCreatedUtc
            };
        }
    }
}
