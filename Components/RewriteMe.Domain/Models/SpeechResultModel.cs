using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Models
{
    public class SpeechResultModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public long Ticks { get; set; }
    }
}
