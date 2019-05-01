using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Business.Services
{
    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;
        private readonly IFileItemRepository _fileItemRepository;

        public UserSubscriptionService(
            IUserSubscriptionRepository userSubscriptionRepository,
            IFileItemRepository fileItemRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
            _fileItemRepository = fileItemRepository;
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
    }
}
