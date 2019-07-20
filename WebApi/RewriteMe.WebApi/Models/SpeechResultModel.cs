using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Models
{
    public class SpeechResultModel
    {
        [Required]
        public Guid Id { get; set; }

        public TimeSpan TotalTime { get; set; }
    }
}
