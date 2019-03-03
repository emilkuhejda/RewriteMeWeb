using System;

namespace RewriteMe.Domain.Transcription
{
    public class FileItem
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public byte[] Source { get; set; }

        public string ContentType { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }
    }
}
