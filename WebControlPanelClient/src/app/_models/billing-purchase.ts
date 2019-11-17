export class BillingPurchase {
    id: string;
    userId: string;
    purchaseId: string;
    productId: string;
    autoRenewing: string;
    purchaseToken: string;
    purchaseState: string;
    consumptionState: string;
    platform: string;
    transactionDateUtc: Date;
}
