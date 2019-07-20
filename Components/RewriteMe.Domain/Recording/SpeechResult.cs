using System;

namespace RewriteMe.Domain.Recording
{
    public class SpeechResult
    {
        public Guid Id { get; set; }

        public Guid RecognizedAudioSampleId { get; set; }

        public string DisplayText { get; set; }

        public TimeSpan TotalTime { get; set; }
    }
}
