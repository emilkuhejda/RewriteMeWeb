using System;
using System.Collections.Generic;

namespace RewriteMe.Domain.Transcription
{
    public class TranscribeItem
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public Guid ApplicationId { get; set; }

        public IEnumerable<RecognitionAlternative> Alternatives { get; set; }

        public string UserTranscript { get; set; }

        public string SourceFileName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan TotalTime { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public DateTime DateUpdatedUtc { get; set; }
    }
}
