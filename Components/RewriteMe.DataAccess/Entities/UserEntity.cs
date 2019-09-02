using System;
using System.Collections.Generic;

namespace RewriteMe.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public Guid ApplicationId { get; set; }

        public DateTime DateRegistered { get; set; }

        public virtual IEnumerable<FileItemEntity> FileItems { get; set; }

        public virtual IEnumerable<RecognizedAudioSampleEntity> RecognizedAudioSamples { get; set; }

        public virtual IEnumerable<UserSubscriptionEntity> UserSubscriptions { get; set; }

        public virtual IEnumerable<BillingPurchaseEntity> BillingPurchases { get; set; }

        public virtual IEnumerable<ApplicationLogEntity> ApplicationLogs { get; set; }

        public virtual IEnumerable<UserDeviceEntity> UserDevices { get; set; }
    }
}
