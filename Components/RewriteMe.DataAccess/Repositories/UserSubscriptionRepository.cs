﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

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

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.CurrentUserSubscription
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.DateUpdatedUtc)
                    .Select(x => x.DateUpdatedUtc)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task AddAndRecalculateUserSubscriptionAsync(UserSubscription userSubscription)
        {
            await SemaphoreSlim.WaitAsync().ConfigureAwait(true);
            try
            {
                using (var context = _contextFactory.Create())
                {
                    await RecalculateUserSubscription(context, userSubscription).ConfigureAwait(false);
                    await context.UserSubscriptions.AddAsync(userSubscription.ToUserSubscriptionEntity())
                        .ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                SemaphoreSlim.Release();
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
                Ticks = ticks,
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

        public async Task RecalculateCurrentUserSubscriptions()
        {
            using (var context = _contextFactory.Create())
            {
                var currentUserSubscriptions = await context.CurrentUserSubscription.ToListAsync().ConfigureAwait(false);
                foreach (var currentUserSubscription in currentUserSubscriptions)
                {
                    var userSubscriptions = await context.UserSubscriptions
                        .AsNoTracking()
                        .Where(x => x.UserId == currentUserSubscription.UserId)
                        .ToListAsync()
                        .ConfigureAwait(false);
                    currentUserSubscription.Ticks = userSubscriptions.CalculateRemainingTicks();
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
