using System;

namespace RewriteMe.DataAccess.Entities
{
    public class TranscribeItemEntity
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public string Alternatives { get; set; }

        public byte[] Source { get; set; }

        public TimeSpan TotalTime { get; set; }

        public FileItemEntity FileItem { get; set; }
    }
}
