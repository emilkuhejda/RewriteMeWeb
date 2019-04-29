using System;

namespace RewriteMe.WebApi.Dtos
{
    public class FileItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Language { get; set; }

        public string RecognitionState { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }

        public int Version { get; set; }
    }
}
