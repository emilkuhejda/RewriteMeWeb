﻿using System;

namespace RewriteMe.Domain.Transcription
{
    public class BillingPurchase
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string PurchaseId { get; set; }

        public string ProductId { get; set; }

        public bool AutoRenewing { get; set; }

        public string PurchaseToken { get; set; }

        public string PurchaseState { get; set; }

        public string ConsumptionState { get; set; }

        public string Platform { get; set; }

        public DateTime TransactionDateUtc { get; set; }
    }
}
