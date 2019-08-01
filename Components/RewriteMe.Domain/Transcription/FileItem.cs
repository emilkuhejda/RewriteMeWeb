﻿using System;
using System.Collections.Generic;
using System.IO;
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

        public string OriginalSourcePath => string.IsNullOrWhiteSpace(OriginalSourceFileName)
            ? string.Empty
            : Path.Combine(Id.ToString(), OriginalSourceFileName);

        public string SourceFileName { get; set; }

        public string SourcePath => string.IsNullOrWhiteSpace(SourceFileName)
            ? string.Empty
            : Path.Combine(Id.ToString(), SourceFileName);

        public string OriginalContentType { get; set; }

        public TimeSpan TotalTime { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<TranscribeItem> TranscribeItems { get; set; }
    }
}
