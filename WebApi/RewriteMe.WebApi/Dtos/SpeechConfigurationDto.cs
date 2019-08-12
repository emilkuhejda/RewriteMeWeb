using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class SpeechConfigurationDto
    {
        [Required]
        public string SubscriptionKey { get; set; }

        [Required]
        public string SpeechRegion { get; set; }

        [Required]
        public Guid AudioSampleId { get; set; }

        [Required]
        public long SubscriptionRemainingTimeTicks { get; set; }
    }
}
