using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class OkDto
    {
        public OkDto()
            : this(DateTime.UtcNow)
        {
        }

        public OkDto(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        [Required]
        public DateTime DateTime { get; }
    }
}
