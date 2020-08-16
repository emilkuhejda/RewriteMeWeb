using System;

namespace RewriteMe.DataAccess.Entities
{
    public class WavPartialFileEntity
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public string Path { get; set; }

        public int AudioChannels { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan TotalTime { get; set; }

        public virtual FileItemEntity FileItem { get; set; }
    }
}
