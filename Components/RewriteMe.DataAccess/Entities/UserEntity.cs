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

        public DateTime DateRegisteredUtc { get; set; }

        public virtual CurrentUserSubscriptionEntity CurrentUserSubscription { get; set; }

        public virtual IList<UserSubscriptionEntity> UserSubscriptions { get; set; }

        public virtual IList<FileItemEntity> FileItems { get; set; }

        public virtual IList<RecognizedAudioSampleEntity> RecognizedAudioSamples { get; set; }

        public virtual IList<BillingPurchaseEntity> BillingPurchases { get; set; }

        public virtual IList<ApplicationLogEntity> ApplicationLogs { get; set; }

        public virtual IList<InformationMessageEntity> InformationMessages { get; set; }

        public virtual IList<UserDeviceEntity> UserDevices { get; set; }
    }
}
