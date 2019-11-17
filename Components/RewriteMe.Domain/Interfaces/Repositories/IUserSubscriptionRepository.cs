using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUserSubscriptionRepository
    {
        Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId);

        Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAndRecalculateUserSubscriptionAsync(UserSubscription userSubscription);

        Task<TimeSpan> GetTotalSubscriptionTimeAsync(Guid userId);

        Task<TimeSpan> GetRemainingTimeAsync(Guid userId);
    }
}
