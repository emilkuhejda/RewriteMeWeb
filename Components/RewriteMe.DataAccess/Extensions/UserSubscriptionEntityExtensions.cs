using System;
using System.Collections.Generic;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Enums;

namespace RewriteMe.DataAccess.Extensions
{
    public static class UserSubscriptionEntityExtensions
    {
        public static long CalculateRemainingTicks(this List<UserSubscriptionEntity> userSubscriptions)
        {
            var time = TimeSpan.Zero;

            foreach (var userSubscription in userSubscriptions)
            {
                if (userSubscription.Operation == SubscriptionOperation.Add)
                {
                    time = time.Add(userSubscription.Time);
                }
                else if (userSubscription.Operation == SubscriptionOperation.Remove)
                {
                    time = time.Subtract(userSubscription.Time);
                }
                else
                {
                    throw new NotSupportedException(nameof(userSubscription.Operation));
                }
            }

            return time.Ticks;
        }
    }
}
