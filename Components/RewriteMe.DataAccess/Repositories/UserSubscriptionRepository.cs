using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.DataAccess.Entities;
using RewriteMe.DataAccess.Extensions;
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
                    .Where(x => x.UserId == userId && x.DateCreatedUtc >= updatedAfter && x.ApplicationId != applicationId)
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
                    .OrderByDescending(x => x.DateCreatedUtc)
                    .Select(x => x.DateCreatedUtc)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task AddAndRecalculateUserSubscriptionAsync(UserSubscription userSubscription)
        {
            using (var context = _contextFactory.Create())
            {
                await RecalculateUserSubscription(context, userSubscription).ConfigureAwait(false);
                await context.UserSubscriptions.AddAsync(userSubscription.ToUserSubscriptionEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task RecalculateUserSubscription(AppDbContext context, UserSubscription userSubscription)
        {
            var userSubscriptions = await context.UserSubscriptions
                .AsNoTracking()
                .Where(x => x.UserId == userSubscription.UserId)
                .ToListAsync()
                .ConfigureAwait(false);
            userSubscriptions.Add(userSubscription.ToUserSubscriptionEntity());
            var ticks = userSubscriptions.CalculateRemainingTicks();

            var current = new CurrentUserSubscriptionEntity
            {
                Id = Guid.NewGuid(),
                UserId = userSubscription.UserId,
                Time = TimeSpan.FromTicks(ticks),
                DateUpdatedUtc = DateTime.UtcNow
            };

            var subscriptions = await context.CurrentUserSubscription
                .Where(x => x.UserId == userSubscription.UserId)
                .ToListAsync()
                .ConfigureAwait(false);

            if (subscriptions.Any())
            {
                context.CurrentUserSubscription.RemoveRange(subscriptions);
            }

            await context.CurrentUserSubscription.AddAsync(current).ConfigureAwait(false);
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

        public async Task<TimeSpan> GetRemainingTimeAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.CurrentUserSubscription.SingleOrDefaultAsync(x => x.UserId == userId).ConfigureAwait(false);
                if (entity == null)
                    return TimeSpan.Zero;

                return entity.Time;
            }
        }
    }
}
