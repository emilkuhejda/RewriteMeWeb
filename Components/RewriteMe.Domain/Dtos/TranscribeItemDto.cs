using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Dtos
{
    public class TranscribeItemDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid FileItemId { get; set; }

        [Required]
        public IEnumerable<RecognitionAlternative> Alternatives { get; set; }

        public string UserTranscript { get; set; }

        [Required]
        public long StartTimeTicks { get; set; }

        [Required]
        public long EndTimeTicks { get; set; }

        [Required]
        public long TotalTimeTicks { get; set; }

        [Required]
        public DateTime DateCreatedUtc { get; set; }

        [Required]
        public DateTime DateUpdatedUtc { get; set; }
    }
}
