using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class RecognizedTimeDto
    {
        [Required]
        public long TotalTimeTicks { get; set; }
    }
}
