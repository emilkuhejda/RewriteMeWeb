using System;

namespace RewriteMe.Domain.Transcription
{
    public static class SubscriptionProducts
    {
        public static SubscriptionProduct Product1Hour { get; } = new SubscriptionProduct("product.subscription.1hour", TimeSpan.FromHours(1));

        public static SubscriptionProduct[] All { get; } = { Product1Hour };

        public static DateTime LastUpdate { get; } = DateTime.SpecifyKind(new DateTime(2018, 1, 1), DateTimeKind.Utc);
    }
}
