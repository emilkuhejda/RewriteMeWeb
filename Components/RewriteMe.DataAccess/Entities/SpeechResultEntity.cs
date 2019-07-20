using System;

namespace RewriteMe.DataAccess.Entities
{
    public class SpeechResultEntity
    {
        public Guid Id { get; set; }

        public Guid RecognizedAudioSampleId { get; set; }

        public string DisplayText { get; set; }

        public TimeSpan TotalTime { get; set; }

        public virtual RecognizedAudioSampleEntity RecognizedAudioSample { get; set; }
    }
}
