using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Dtos
{
    public class LanguageVersionDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid InformationMessageId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string LanguageString { get; set; }

        [Required]
        public bool SentOnOsx { get; set; }

        [Required]
        public bool SentOnAndroid { get; set; }
    }
}
