﻿using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.DataAccess.Entities
{
    public class InformationMessageEntity
    {
        public Guid Id { get; set; }

        public string CampaignName { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Description { get; set; }

        public Language Language { get; set; }

        public bool SentOnOsx { get; set; }

        public bool SentOnAndroid { get; set; }

        public DateTime DateCreated { get; set; }
    }
}