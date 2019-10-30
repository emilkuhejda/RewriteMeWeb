using System;
using System.Collections.Generic;

namespace RewriteMe.Domain.Recording
{
    public class RecognizedAudioSample
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public IEnumerable<SpeechResult> SpeechResults { get; set; }
    }
}
