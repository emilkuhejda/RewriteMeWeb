using System;

namespace RewriteMe.Domain.Transcription
{
    public class AudioSource
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public byte[] OriginalSource { get; set; }

        public byte[] WavSource { get; set; }

        public string ContentType { get; set; }

        public TimeSpan TotalTime { get; set; }

        public int Version { get; set; }
    }
}
