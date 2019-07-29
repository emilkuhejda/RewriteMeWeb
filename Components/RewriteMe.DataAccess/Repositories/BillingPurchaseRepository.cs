using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class BillingPurchaseRepository : IBillingPurchaseRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public BillingPurchaseRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(BillingPurchase billingPurchase)
        {
            using (var context = _contextFactory.Create())
            {
                await context.BillingPurchases.AddAsync(billingPurchase.ToBillingPurchaseEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<BillingPurchase>> GetAllByUserAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var billingPurchases = await context.BillingPurchases
                    .Where(x => x.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return billingPurchases.Select(x => x.ToBillingPurchase());
            }
        }
    }
}
