using System;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.WebApi.Commands
{
    public class CreateUserSubscriptionCommand : CommandBase<TimeSpanWrapperDto>
    {
        public BillingPurchase BillingPurchase { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
