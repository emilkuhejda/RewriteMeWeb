using System;
using RewriteMe.Domain.Notifications;

namespace RewriteMe.Domain.Settings
{
    public class AppSettings
    {
        public Guid ApplicationId { get; set; }

        public string SecretKey { get; set; }

        public string HangfireSecretKey { get; set; }

        public string ConnectionString { get; set; }

        public string SecurityPasswordHash { get; set; }

        public AzureStorageAccountSettings AzureStorageAccount { get; set; }

        public NotificationSettings NotificationSettings { get; set; }

        public Authentication Authentication { get; set; }

        public string GoogleApiAuthUri { get; set; }

        public SpeechCredentials SpeechCredentials { get; set; }

        public string AzureSubscriptionKey { get; set; }

        public string AzureSpeechRegion { get; set; }
    }
}
