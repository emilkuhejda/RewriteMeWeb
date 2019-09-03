using System;
using System.Collections.Generic;

namespace RewriteMe.DataAccess.Entities
{
    public class InformationMessageEntity
    {
        public Guid Id { get; set; }

        public string CampaignName { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual IList<LanguageVersionEntity> LanguageVersions { get; set; }
    }
}
