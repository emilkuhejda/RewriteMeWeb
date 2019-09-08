using System;
using System.Collections.Generic;

namespace RewriteMe.Domain.Messages
{
    public class InformationMessage
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string CampaignName { get; set; }

        public bool WasOpened { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DatePublished { get; set; }

        public virtual IList<LanguageVersion> LanguageVersions { get; set; }
    }
}
