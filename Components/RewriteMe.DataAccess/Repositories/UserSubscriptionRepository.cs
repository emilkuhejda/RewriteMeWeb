using System.Threading.Tasks;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Settings;

namespace RewriteMe.DataAccess.Repositories
{
    public class UserSubscriptionRepository : IUserSubscriptionRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public UserSubscriptionRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(UserSubscription userSubscription)
        {
            using (var context = _contextFactory.Create())
            {
                await context.UserSubscriptions.AddAsync(userSubscription.ToUserSubscriptionEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
