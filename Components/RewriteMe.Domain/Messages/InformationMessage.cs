using System;

namespace RewriteMe.Domain.Messages
{
    public class InformationMessage
    {
        public Guid Id { get; set; }

        public string CampaignName { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public bool SentOnOsx { get; set; }

        public bool SentOnAndroid { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
