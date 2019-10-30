using System;

namespace RewriteMe.DataAccess.Entities
{
    public class TranscribeItemEntity
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public Guid ApplicationId { get; set; }

        public string Alternatives { get; set; }

        public string UserTranscript { get; set; }

        public string SourceFileName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan TotalTime { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public DateTime DateUpdatedUtc { get; set; }

        public virtual FileItemEntity FileItem { get; set; }
    }
}
