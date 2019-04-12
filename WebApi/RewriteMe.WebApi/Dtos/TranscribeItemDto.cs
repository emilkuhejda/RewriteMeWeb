using System;
using System.Collections.Generic;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.WebApi.Dtos
{
    public class TranscribeItemDto
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public IEnumerable<RecognitionAlternative> Alternatives { get; set; }

        public string UserTranscript { get; set; }

        public byte[] Source { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan TotalTime { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
