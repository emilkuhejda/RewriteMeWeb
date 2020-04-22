using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RewriteMe.DataAccess.Entities
{
    public class CurrentUserSubscriptionEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public long Ticks { get; set; }

        [NotMapped]
        public TimeSpan Time => TimeSpan.FromTicks(Ticks);

        public DateTime DateUpdatedUtc { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
