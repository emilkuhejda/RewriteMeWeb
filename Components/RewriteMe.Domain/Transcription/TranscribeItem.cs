using System;

namespace RewriteMe.Domain.Transcription
{
    public class TranscribeItem
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public string Transcript { get; set; }

        public byte[] Source { get; set; }
    }
}
