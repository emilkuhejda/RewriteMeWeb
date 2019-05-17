using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;
        private readonly IBillingPurchaseRepository _billingPurchaseRepository;
        private readonly IFileItemRepository _fileItemRepository;

        public UserSubscriptionService(
            IUserSubscriptionRepository userSubscriptionRepository,
            IBillingPurchaseRepository billingPurchaseRepository,
            IFileItemRepository fileItemRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
            _billingPurchaseRepository = billingPurchaseRepository;
            _fileItemRepository = fileItemRepository;
        }

        public async Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            return await _userSubscriptionRepository.GetAllAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            return await _fileItemRepository.GetLastUpdateAsync(userId).ConfigureAwait(false);
        }

        public async Task AddAsync(UserSubscription userSubscription)
        {
            await _userSubscriptionRepository.AddAsync(userSubscription).ConfigureAwait(false);
        }

        public async Task<TimeSpan> GetRemainingTime(Guid userId)
        {
            var totalSubscriptionTime = await _userSubscriptionRepository.GetTotalSubscriptionTime(userId).ConfigureAwait(false);
            var transcribedTotalSeconds = await _fileItemRepository.GetTranscribedTotalSeconds(userId).ConfigureAwait(false);

            return totalSubscriptionTime.Subtract(transcribedTotalSeconds);
        }

        public async Task<UserSubscription> RegisterPurchaseAsync(BillingPurchase billingPurchase, Guid applicationId)
        {
            await _billingPurchaseRepository.AddAsync(billingPurchase).ConfigureAwait(false);
            var subscriptionProduct = SubscriptionProducts.All.FirstOrDefault(x => x.Id == billingPurchase.ProductId);
            if (subscriptionProduct == null)
                return null;

            var userSubscription = new UserSubscription
            {
                Id = Guid.NewGuid(),
                UserId = billingPurchase.UserId,
                ApplicationId = applicationId,
                Time = subscriptionProduct.Time,
                DateCreated = DateTime.UtcNow
            };

            await _userSubscriptionRepository.AddAsync(userSubscription).ConfigureAwait(false);

            return userSubscription;
        }
    }
}
