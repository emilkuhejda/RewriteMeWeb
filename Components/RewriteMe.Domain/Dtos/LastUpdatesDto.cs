using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Dtos
{
    public class LastUpdatesDto
    {
        [Required]
        public DateTime FileItemUtc { get; set; }

        [Required]
        public DateTime DeletedFileItemUtc { get; set; }

        [Required]
        public DateTime TranscribeItemUtc { get; set; }

        [Required]
        public DateTime UserSubscriptionUtc { get; set; }

        [Required]
        public DateTime InformationMessageUtc { get; set; }
    }
}
