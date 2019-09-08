using System;
using System.Collections.Generic;

namespace RewriteMe.DataAccess.Entities
{
    public class InformationMessageEntity
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string CampaignName { get; set; }

        public bool WasOpened { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DatePublished { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual IList<LanguageVersionEntity> LanguageVersions { get; set; }
    }
}
