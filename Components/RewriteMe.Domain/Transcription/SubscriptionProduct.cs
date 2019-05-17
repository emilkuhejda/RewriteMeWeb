using System;

namespace RewriteMe.Domain.Transcription
{
    public class SubscriptionProduct
    {
        public SubscriptionProduct(string id, TimeSpan time)
        {
            Id = id;
            Time = time;
        }

        public string Id { get; }

        public TimeSpan Time { get; }
    }
}
