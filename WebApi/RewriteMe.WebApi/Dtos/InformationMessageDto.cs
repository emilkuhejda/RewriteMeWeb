using System;
using System.Collections.Generic;
using RewriteMe.Domain.Messages;

namespace RewriteMe.WebApi.Dtos
{
    public class InformationMessageDto
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<LanguageVersion> LanguageVersions { get; set; }
    }
}
