using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.WebApi.Dtos
{
    public class TranscribeItemDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid FileItemId { get; set; }

        [Required]
        public IEnumerable<RecognitionAlternative> Alternatives { get; set; }

        [Required]
        public string UserTranscript { get; set; }

        [Required]
        [MaxLength(50)]
        public string StartTimeString { get; set; }

        [Required]
        [MaxLength(50)]
        public string EndTimeString { get; set; }

        [Required]
        [MaxLength(50)]
        public string TotalTimeString { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }
    }
}
