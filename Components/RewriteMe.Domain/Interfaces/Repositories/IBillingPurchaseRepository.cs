using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IBillingPurchaseRepository
    {
        Task AddAsync(BillingPurchase billingPurchase);

        Task<IEnumerable<BillingPurchase>> GetAllByUserAsync(Guid userId);

        Task<BillingPurchase> GetAsync(Guid purchaseId);
    }
}
