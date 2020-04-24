using System;
using System.Collections.Generic;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Transcription
{
    public class FileItem
    {
        public FileItem()
        {
            RecognitionState = RecognitionState.None;
        }

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ApplicationId { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Language { get; set; }

        public RecognitionState RecognitionState { get; set; }

        public string OriginalSourceFileName { get; set; }

        public string SourceFileName { get; set; }

        public StorageSetting Storage { get; set; }

        public UploadStatus UploadStatus { get; set; }

        public TimeSpan TotalTime { get; set; }

        public TimeSpan TranscribedTime { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessedUtc { get; set; }

        public DateTime DateUpdatedUtc { get; set; }

        public bool IsDeleted { get; set; }

        public bool WasCleaned { get; set; }

        public bool CanMigrate => RecognitionState != RecognitionState.Converting ||
                                  RecognitionState != RecognitionState.InProgress;

        public IEnumerable<TranscribeItem> TranscribeItems { get; set; }
    }
}
