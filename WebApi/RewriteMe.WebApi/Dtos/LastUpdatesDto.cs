using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class LastUpdatesDto
    {
        [Required]
        public DateTime FileItem { get; set; }

        [Required]
        public DateTime TranscribeItem { get; set; }

        [Required]
        public DateTime UserSubscription { get; set; }
    }
}
