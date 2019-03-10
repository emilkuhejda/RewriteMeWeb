﻿using System;
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

        public string Name { get; set; }

        public string FileName { get; set; }

        public byte[] Source { get; set; }

        public string ContentType { get; set; }

        public RecognitionState RecognitionState { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }

        public IEnumerable<TranscribeItem> TranscribeItems { get; set; }
    }
}
