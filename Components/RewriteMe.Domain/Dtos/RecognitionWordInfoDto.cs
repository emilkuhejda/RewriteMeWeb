using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Dtos
{
    public class RecognitionWordInfoDto
    {
        [Required]
        public string Word { get; set; }

        [Required]
        public long StartTimeTicks { get; set; }

        [Required]
        public long EndTimeTicks { get; set; }

        [Required]
        public int SpeakerTag { get; set; }
    }
}
