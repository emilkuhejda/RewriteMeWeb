using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var userSubscriptions = await context.UserSubscriptions
                    .Where(x => x.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return userSubscriptions.Select(x => x.ToUserSubscription());
            }
        }

        public async Task<IEnumerable<UserSubscription>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var userSubscriptions = await context.UserSubscriptions
                    .Where(x => x.UserId == userId && x.DateCreated >= updatedAfter && x.ApplicationId != applicationId)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return userSubscriptions.Select(x => x.ToUserSubscription());
            }
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.UserSubscriptions
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.DateCreated)
                    .Select(x => x.DateCreated)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task AddAsync(UserSubscription userSubscription)
        {
            using (var context = _contextFactory.Create())
            {
                await context.UserSubscriptions.AddAsync(userSubscription.ToUserSubscriptionEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<TimeSpan> GetTotalSubscriptionTimeAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var remainingTicks = await context.UserSubscriptions
                    .Where(x => x.UserId == userId)
                    .Select(x => x.Time.Ticks)
                    .SumAsync(x => x)
                    .ConfigureAwait(false);

                return TimeSpan.FromTicks(remainingTicks);
            }
        }
    }
}
