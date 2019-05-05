using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IUserSubscriptionService
    {
        Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId, DateTime updatedAfter);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAsync(UserSubscription userSubscription);

        Task<TimeSpan> GetRemainingTime(Guid userId);
    }
}
