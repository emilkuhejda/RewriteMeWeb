using System;

namespace RewriteMe.Domain.Settings
{
    public class UserSubscription
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ApplicationId { get; set; }

        public TimeSpan Time { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
