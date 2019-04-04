using System;

namespace RewriteMe.DataAccess.Entities
{
    public class AudioSourceEntity
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public byte[] OriginalSource { get; set; }

        public byte[] WavSource { get; set; }

        public string ContentType { get; set; }

        public TimeSpan TotalTime { get; set; }

        public virtual FileItemEntity FileItem { get; set; }
    }
}
