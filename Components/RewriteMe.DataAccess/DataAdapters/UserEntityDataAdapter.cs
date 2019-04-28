using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Settings;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class UserEntityDataAdapter
    {
        public static UserSubscription ToUserSubscription(this UserSubscriptionEntity entity)
        {
            return new UserSubscription
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Time = entity.Time,
                DateCreated = entity.DateCreated
            };
        }

        public static UserSubscriptionEntity ToUserSubscriptionEntity(this UserSubscription userSubscription)
        {
            return new UserSubscriptionEntity
            {
                Id = userSubscription.Id,
                UserId = userSubscription.UserId,
                Time = userSubscription.Time,
                DateCreated = userSubscription.DateCreated
            };
        }
    }
}
