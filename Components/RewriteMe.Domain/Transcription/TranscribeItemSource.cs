﻿using System;

namespace RewriteMe.Domain.Transcription
{
    public class TranscribeItemSource
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public byte[] Source { get; set; }

        public DateTime DateCreatedUtc { get; set; }
    }
}
