using System;

namespace RewriteMe.Domain.Transcription
{
    public class WavPartialFile
    {
        public Guid Id { get; set; }

        public string Path { get; set; }

        public int AudioChannels { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan TotalTime { get; set; }
    }
}
