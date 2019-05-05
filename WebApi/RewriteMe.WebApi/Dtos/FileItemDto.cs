using System;

namespace RewriteMe.WebApi.Dtos
{
    public class FileItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Language { get; set; }

        public string RecognitionStateString { get; set; }

        public string TotalTimeString { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }

        public DateTime DateUpdated { get; set; }

        public int AudioSourceVersion { get; set; }

        public AudioSourceDto AudioSource { get; set; }
    }
}
