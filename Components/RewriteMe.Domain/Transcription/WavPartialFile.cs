using System;

namespace RewriteMe.Domain.Transcription
{
    public class WavPartialFile
    {
        public WavPartialFile(string path, TimeSpan totalTime)
        {
            Path = path;
            TotalTime = totalTime;
        }

        public string Path { get; }

        public TimeSpan TotalTime { get; }
    }
}
