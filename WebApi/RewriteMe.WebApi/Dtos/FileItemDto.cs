using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class FileItemDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string RecognitionStateString { get; set; }

        [Required]
        public long TotalTimeTicks { get; set; }

        [Required]
        public long TranscribedTimeTicks { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
