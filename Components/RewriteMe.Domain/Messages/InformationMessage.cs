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

        public DateTime DateCreatedUtc { get; set; }

        public DateTime? DateUpdatedUtc { get; set; }

        public DateTime? DatePublishedUtc { get; set; }

        public virtual IList<LanguageVersion> LanguageVersions { get; set; }
    }
}
