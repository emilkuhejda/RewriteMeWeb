using System;

namespace RewriteMe.Domain.Transcription
{
    public class UploadedChunk
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public byte[] Source { get; set; }

        public int Order { get; set; }

        public DateTime DateCreatedUtc { get; set; }
    }
}
