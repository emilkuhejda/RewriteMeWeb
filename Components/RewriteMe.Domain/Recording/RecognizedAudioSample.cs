using System;
using System.Collections.Generic;

namespace RewriteMe.Domain.Recording
{
    public class RecognizedAudioSample
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<SpeechResult> SpeechResults { get; set; }
    }
}
