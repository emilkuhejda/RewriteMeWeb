using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Models
{
    public class UpdateTranscribeItemModel
    {
        [Required]
        public Guid TranscribeItemId { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public string Transcript { get; set; }
    }
}
