using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class SpeechConfigurationDto
    {
        [Required]
        [MaxLength(50)]
        public string SubscriptionKey { get; set; }

        [Required]
        [MaxLength(50)]
        public string SpeechRegion { get; set; }

        [Required]
        public Guid AudioSampleId { get; set; }

        [Required]
        [MaxLength(50)]
        public long SubscriptionRemainingTimeTicks { get; set; }
    }
}
