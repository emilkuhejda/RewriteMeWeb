using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Dtos
{
    public class RecognitionAlternativeDto
    {
        [Required]
        public string Transcript { get; set; }

        [Required]
        public float Confidence { get; set; }

        [Required]
        public IEnumerable<RecognitionWordInfoDto> Words { get; set; }
    }
}
