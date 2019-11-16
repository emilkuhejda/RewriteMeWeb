using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class RemainingTimeDto
    {
        [Required]
        public long TimeTicks { get; set; }
    }
}
