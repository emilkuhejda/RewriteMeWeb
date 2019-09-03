using Newtonsoft.Json;

namespace RewriteMe.Domain.Notifications
{
    [JsonObject]
    public class NotificationResult
    {
        [JsonProperty("notification_id")]
        public string Id { get; set; }
    }
}
