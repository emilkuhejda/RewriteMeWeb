using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class TimeSpanWrapperDto
    {
        [Required]
        public long Ticks { get; set; }
    }
}
