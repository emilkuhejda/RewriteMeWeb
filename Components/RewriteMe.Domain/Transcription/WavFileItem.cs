using System;

namespace RewriteMe.Domain.Transcription
{
    public class WavFileItem
    {
        public WavFileItem(string path, TimeSpan totalTime)
        {
            Path = path;
            TotalTime = totalTime;
        }

        public string Path { get; }

        public TimeSpan TotalTime { get; }
    }
}
