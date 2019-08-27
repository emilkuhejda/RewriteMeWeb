﻿using System;

namespace RewriteMe.DataAccess.Entities
{
    public class InformationMessageEntity
    {
        public Guid Id { get; set; }

        public string CampaignName { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
