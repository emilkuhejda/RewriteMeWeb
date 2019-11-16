using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IUserSubscriptionService
    {
        Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId);

        Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAsync(UserSubscription userSubscription);

        Task<TimeSpan> GetRemainingTimeAsync(Guid userId);

        Task<TimeSpan> GetCalculatedRemainingTimeAsync(Guid userId);

        Task<UserSubscription> RegisterPurchaseAsync(BillingPurchase billingPurchase, Guid applicationId);
    }
}
