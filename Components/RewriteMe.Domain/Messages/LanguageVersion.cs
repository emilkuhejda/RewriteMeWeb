using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Messages
{
    public class LanguageVersion
    {
        public Guid Id { get; set; }

        public Guid InformationMessageId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Description { get; set; }

        public Language Language { get; set; }

        public bool SentOnOsx { get; set; }

        public bool SentOnAndroid { get; set; }
    }
}
