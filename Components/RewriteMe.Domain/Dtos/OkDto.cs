using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Dtos
{
    public class OkDto
    {
        public OkDto()
            : this(DateTime.UtcNow)
        {
        }

        public OkDto(DateTime dateTimeUtc)
        {
            DateTimeUtc = dateTimeUtc;
        }

        [Required]
        public DateTime DateTimeUtc { get; }
    }
}
