using System;

namespace RewriteMe.DataAccess.Entities
{
    public class CurrentUserSubscriptionEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public TimeSpan Time { get; set; }

        public DateTime DateUpdatedUtc { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
