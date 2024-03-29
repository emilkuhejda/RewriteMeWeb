﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUserSubscriptionRepository
    {
        Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAndRecalculateUserSubscriptionAsync(UserSubscription userSubscription);

        Task<TimeSpan> GetRemainingTimeAsync(Guid userId);

        Task RecalculateCurrentUserSubscriptions();
    }
}
