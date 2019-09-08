using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class InformationMessageDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool IsUserSpecific { get; set; }

        [Required]
        public bool WasOpened { get; set; }

        [Required]
        public DateTime? DateUpdated { get; set; }

        [Required]
        public DateTime DatePublished { get; set; }

        public IEnumerable<LanguageVersionDto> LanguageVersions { get; set; }
    }
}
