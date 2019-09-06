using System;
using System.Collections.Generic;

namespace RewriteMe.Domain.Messages
{
    public class InformationMessage
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string CampaignName { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual IEnumerable<LanguageVersion> LanguageVersions { get; set; }
    }
}
