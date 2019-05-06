using System;

namespace RewriteMe.DataAccess.Entities
{
    public class UserSubscriptionEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ApplicationId { get; set; }

        public TimeSpan Time { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
