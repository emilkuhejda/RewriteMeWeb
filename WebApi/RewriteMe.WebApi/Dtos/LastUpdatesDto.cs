using System;

namespace RewriteMe.WebApi.Dtos
{
    public class LastUpdatesDto
    {
        public DateTime FileItem { get; set; }

        public DateTime TranscribeItem { get; set; }

        public DateTime UserSubscription { get; set; }

        public DateTime SubscriptionProduct { get; set; }
    }
}
