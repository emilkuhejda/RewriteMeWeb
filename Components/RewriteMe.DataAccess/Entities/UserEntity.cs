using System;
using System.Collections.Generic;

namespace RewriteMe.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public virtual IEnumerable<FileItemEntity> FileItems { get; set; }

        public virtual IEnumerable<UserSubscriptionEntity> UserSubscriptions { get; set; }

        public virtual IEnumerable<ApplicationLogEntity> ApplicationLogs { get; set; }
    }
}
