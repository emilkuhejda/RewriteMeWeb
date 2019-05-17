using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class BillingPurchaseDataAdapter
    {
        public static BillingPurchase ToBillingPurchase(this BillingPurchaseEntity entity)
        {
            return new BillingPurchase
            {
                Id = entity.Id,
                UserId = entity.UserId,
                PurchaseId = entity.PurchaseId,
                ProductId = entity.ProductId,
                AutoRenewing = entity.AutoRenewing,
                PurchaseState = entity.PurchaseState,
                ConsumptionState = entity.ConsumptionState,
                Platform = entity.Platform,
                TransactionDateUtc = entity.TransactionDateUtc
            };
        }

        public static BillingPurchaseEntity ToBillingPurchaseEntity(this BillingPurchase billingPurchase)
        {
            return new BillingPurchaseEntity
            {
                Id = billingPurchase.Id,
                UserId = billingPurchase.UserId,
                PurchaseId = billingPurchase.PurchaseId,
                ProductId = billingPurchase.ProductId,
                AutoRenewing = billingPurchase.AutoRenewing,
                PurchaseState = billingPurchase.PurchaseState,
                ConsumptionState = billingPurchase.ConsumptionState,
                Platform = billingPurchase.Platform,
                TransactionDateUtc = billingPurchase.TransactionDateUtc
            };
        }
    }
}
