using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Dtos
{
    public class RecognizedTimeDto
    {
        [Required]
        public long TotalTimeTicks { get; set; }
    }
}
