using System;
using System.Collections.Generic;
using RewriteMe.Domain.Enums;

namespace RewriteMe.DataAccess.Entities
{
    public class FileItemEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ApplicationId { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Language { get; set; }

        public RecognitionState RecognitionState { get; set; }

        public string OriginalSourceFileName { get; set; }

        public string SourceFileName { get; set; }

        public string OriginalContentType { get; set; }

        public TimeSpan TotalTime { get; set; }

        public TimeSpan TranscribedTime { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPermanentlyDeleted { get; set; }

        public bool WasCleaned { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual FileItemSourceEntity FileItemSource { get; set; }

        public virtual IList<TranscribeItemEntity> TranscribeItems { get; set; }

        public virtual IList<TranscribeItemSourceEntity> TranscribeItemSources { get; set; }
    }
}
