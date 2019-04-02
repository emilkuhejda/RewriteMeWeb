﻿using System;
using System.Collections.Generic;

namespace RewriteMe.DataAccess.Entities
{
    public class UserEntity
    {
        public UserEntity()
        {
            FileItems = new HashSet<FileItemEntity>();
            UserSubscriptions = new HashSet<UserSubscriptionEntity>();
            ApplicationLogs = new HashSet<ApplicationLogEntity>();
        }

        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public IEnumerable<FileItemEntity> FileItems { get; set; }

        public IEnumerable<UserSubscriptionEntity> UserSubscriptions { get; set; }

        public IEnumerable<ApplicationLogEntity> ApplicationLogs { get; set; }
    }
}
