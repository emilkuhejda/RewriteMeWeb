﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IBillingPurchaseService
    {
        Task<IEnumerable<BillingPurchase>> GetAllByUserAsync(Guid userId);

        Task<BillingPurchase> GetAsync(Guid purchaseId);
    }
}
