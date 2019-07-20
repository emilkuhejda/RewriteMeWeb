using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class RecognizedTimeDto
    {
        [Required]
        public string TotalTimeString { get; set; }
    }
}
