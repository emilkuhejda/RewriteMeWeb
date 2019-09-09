import { BillingPurchase } from '../_models/billing-purchase';

export class BillingPurchaseMapper {
    public static convertAll(data): BillingPurchase[] {
        let billingPurchases = [];
        for (let item of data) {
            let billingPurchase = BillingPurchaseMapper.convert(item);
            billingPurchases.push(billingPurchase);
        }

        return billingPurchases;
    }

    public static convert(data): BillingPurchase {
        if (data === null || data === undefined)
            return null;

        let billingPurchase = new BillingPurchase();
        billingPurchase.id = data.id;
        billingPurchase.userId = data.userId;
        billingPurchase.purchaseId = data.purchaseId;
        billingPurchase.productId = data.productId;
        billingPurchase.autoRenewing = data.autoRenewing;
        billingPurchase.purchaseToken = data.purchaseToken;
        billingPurchase.purchaseState = data.purchaseState;
        billingPurchase.consumptionState = data.consumptionState;
        billingPurchase.platform = data.platform;
        billingPurchase.transactionDateUtc = data.transactionDateUtc;

        return billingPurchase;
    }
}
