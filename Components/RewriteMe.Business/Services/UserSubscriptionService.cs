using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Business.Services
{
    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;

        public UserSubscriptionService(IUserSubscriptionRepository userSubscriptionRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        public async Task AddAsync(UserSubscription userSubscription)
        {
            await _userSubscriptionRepository.AddAsync(userSubscription).ConfigureAwait(false);
        }
    }
}
