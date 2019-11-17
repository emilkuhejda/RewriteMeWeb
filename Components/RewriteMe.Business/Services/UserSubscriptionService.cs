using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Enums;
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
        private readonly IRecognizedAudioSampleRepository _recognizedAudioSampleRepository;
        private readonly AppSettings _appSettings;

        public UserSubscriptionService(
            IUserSubscriptionRepository userSubscriptionRepository,
            IBillingPurchaseRepository billingPurchaseRepository,
            IFileItemRepository fileItemRepository,
            IRecognizedAudioSampleRepository recognizedAudioSampleRepository,
            IOptions<AppSettings> options)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
            _billingPurchaseRepository = billingPurchaseRepository;
            _fileItemRepository = fileItemRepository;
            _recognizedAudioSampleRepository = recognizedAudioSampleRepository;
            _appSettings = options.Value;
        }

        public async Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId)
        {
            return await _userSubscriptionRepository.GetAllAsync(userId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            return await _userSubscriptionRepository.GetAllAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            return await _userSubscriptionRepository.GetLastUpdateAsync(userId).ConfigureAwait(false);
        }

        public async Task AddAsync(UserSubscription userSubscription)
        {
            await _userSubscriptionRepository.AddAndRecalculateUserSubscriptionAsync(userSubscription).ConfigureAwait(false);
        }

        public async Task SubtractTimeAsync(Guid userId, TimeSpan time)
        {
            var userSubscription = new UserSubscription
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ApplicationId = _appSettings.ApplicationId,
                Time = time,
                Operation = SubscriptionOperation.Remove,
                DateCreatedUtc = DateTime.UtcNow
            };

            await AddAsync(userSubscription).ConfigureAwait(false);
        }

        public async Task<TimeSpan> GetRemainingTimeAsync(Guid userId)
        {
            return await _userSubscriptionRepository.GetRemainingTimeAsync(userId).ConfigureAwait(false);
        }

        public async Task<TimeSpan> GetCalculatedRemainingTimeAsync(Guid userId)
        {
            var totalSubscriptionTime = await _userSubscriptionRepository.GetTotalSubscriptionTimeAsync(userId).ConfigureAwait(false);
            var transcribedTotalSeconds = await _fileItemRepository.GetTranscribedTotalSecondsAsync(userId).ConfigureAwait(false);
            var realTimeRecognizedTime = await _recognizedAudioSampleRepository.GetRecognizedTimeAsync(userId).ConfigureAwait(false);

            transcribedTotalSeconds = transcribedTotalSeconds.Add(realTimeRecognizedTime);
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
                Operation = SubscriptionOperation.Add,
                DateCreatedUtc = DateTime.UtcNow
            };

            await AddAsync(userSubscription).ConfigureAwait(false);

            return userSubscription;
        }
    }
}
