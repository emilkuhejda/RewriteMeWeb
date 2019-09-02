using System;
using System.Collections.Generic;

namespace RewriteMe.Domain.Messages
{
    public class InformationMessage
    {
        public Guid Id { get; set; }

        public string CampaignName { get; set; }

        public bool SentOnOsx { get; set; }

        public bool SentOnAndroid { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual IEnumerable<LanguageVersion> LanguageVersions { get; set; }
    }
}
