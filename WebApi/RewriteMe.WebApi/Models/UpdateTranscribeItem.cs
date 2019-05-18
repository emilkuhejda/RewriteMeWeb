using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Models
{
    public class UpdateTranscribeItem
    {
        [Required]
        public Guid TranscribeItemId { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public string Transcript { get; set; }
    }
}
