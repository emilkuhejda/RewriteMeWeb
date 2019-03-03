using System;
using System.Collections.Generic;

namespace RewriteMe.Domain.Transcription
{
    public class TranscribeItem
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public IEnumerable<RecognitionAlternative> Alternatives { get; set; }

        public byte[] Source { get; set; }

        public TimeSpan TotalTime { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
