using System;

namespace RewriteMe.Domain.Transcription
{
    public static class SubscriptionProducts
    {
        public static SubscriptionProduct ProductBasic { get; } = new SubscriptionProduct("product.subscription.basic", TimeSpan.FromHours(1));

        public static SubscriptionProduct ProductAdvanced { get; } = new SubscriptionProduct("product.subscription.advanced", TimeSpan.FromHours(10));

        public static SubscriptionProduct[] All { get; } = { ProductBasic, ProductAdvanced };
    }
}
