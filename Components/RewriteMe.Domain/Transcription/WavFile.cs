using System;

namespace RewriteMe.Domain.Transcription
{
    public class WavFile
    {
        public byte[] Source { get; set; }

        public TimeSpan TotalTime { get; set; }
    }
}
