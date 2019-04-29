using System;

namespace RewriteMe.WebApi.Models
{
    public class UpdateTranscribeItem
    {
        public Guid TranscribeItemId { get; set; }

        public string Transcript { get; set; }
    }
}
