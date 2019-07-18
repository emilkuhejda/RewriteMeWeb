using System;

namespace RewriteMe.WebApi.Dtos
{
    public class SpeechConfigurationDto
    {
        public string SubscriptionKey { get; set; }

        public string SpeechRegion { get; set; }

        public Guid AudioSampleId { get; set; }

        public TimeSpan SubscriptionRemainingTime { get; set; }
    }
}
