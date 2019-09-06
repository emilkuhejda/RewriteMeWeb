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
        public DateTime DateCreated { get; set; }

        public IEnumerable<LanguageVersionDto> LanguageVersions { get; set; }
    }
}
