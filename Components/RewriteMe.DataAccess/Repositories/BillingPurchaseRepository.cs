using System.Threading.Tasks;
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
    }
}
