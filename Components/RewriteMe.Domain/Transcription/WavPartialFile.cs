using System;

namespace RewriteMe.Domain.Transcription
{
    public class WavPartialFile
    {
        public string Path { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan TotalTime { get; set; }
    }
}
