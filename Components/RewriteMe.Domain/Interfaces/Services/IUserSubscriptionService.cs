using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IUserSubscriptionService
    {
        Task AddAsync(UserSubscription userSubscription);

        Task<TimeSpan> GetRemainingTime(Guid userId);
    }
}
