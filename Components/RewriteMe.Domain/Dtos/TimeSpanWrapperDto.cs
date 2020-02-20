using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Dtos
{
    public class TimeSpanWrapperDto
    {
        [Required]
        public long Ticks { get; set; }
    }
}
