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

        public DateTime DateCreatedUtc { get; set; }

        public DateTime? DateUpdatedUtc { get; set; }

        public DateTime? DatePublishedUtc { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual IList<LanguageVersionEntity> LanguageVersions { get; set; }
    }
}
