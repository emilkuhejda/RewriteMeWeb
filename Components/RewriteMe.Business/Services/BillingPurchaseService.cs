using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class BillingPurchaseService : IBillingPurchaseService
    {
        private readonly IBillingPurchaseRepository _billingPurchaseRepository;

        public BillingPurchaseService(IBillingPurchaseRepository billingPurchaseRepository)
        {
            _billingPurchaseRepository = billingPurchaseRepository;
        }

        public async Task<IEnumerable<BillingPurchase>> GetAllByUserAsync(Guid userId)
        {
            return await _billingPurchaseRepository.GetAllByUserAsync(userId).ConfigureAwait(false);
        }
    }
}
