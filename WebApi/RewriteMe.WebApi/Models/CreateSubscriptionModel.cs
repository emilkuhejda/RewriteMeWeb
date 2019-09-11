using System;

namespace RewriteMe.WebApi.Models
{
    public class CreateSubscriptionModel
    {
        public Guid UserId { get; set; }

        public Guid ApplicationId { get; set; }

        public int Seconds { get; set; }
    }
}
