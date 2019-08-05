using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class FileItemDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Language { get; set; }

        [Required]
        [MaxLength(20)]
        public string RecognitionStateString { get; set; }

        [Required]
        [MaxLength(50)]
        public long TotalTimeTicks { get; set; }

        [Required]
        [MaxLength(50)]
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
