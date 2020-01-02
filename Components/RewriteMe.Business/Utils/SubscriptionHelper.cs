using System;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Business.Utils
{
    public static class SubscriptionHelper
    {
        public static UserSubscription CreateFreeSubscription(Guid userId, Guid applicationId)
        {
            return new UserSubscription
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ApplicationId = applicationId,
                Time = TimeSpan.FromMinutes(10),
                Operation = SubscriptionOperation.Add,
                DateCreatedUtc = DateTime.UtcNow
            };
        }
    }
}
