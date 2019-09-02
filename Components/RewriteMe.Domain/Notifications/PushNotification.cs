using Newtonsoft.Json;

namespace RewriteMe.Domain.Notifications
{
    [JsonObject]
    public class PushNotification
    {
        [JsonProperty("notification_target")]
        public NotificationTarget Target { get; set; }

        [JsonProperty("notification_content")]
        public NotificationContent Content { get; set; }
    }
}
