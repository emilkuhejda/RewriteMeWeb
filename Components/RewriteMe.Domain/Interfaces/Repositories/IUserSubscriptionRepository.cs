using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUserSubscriptionRepository
    {
        Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId, DateTime updatedAfter);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAsync(UserSubscription userSubscription);

        Task<TimeSpan> GetTotalSubscriptionTime(Guid userId);
    }
}
