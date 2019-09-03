using Newtonsoft.Json;

namespace RewriteMe.Domain.Notifications
{
    [JsonObject]
    public class NotificationErrorWrapper
    {
        [JsonProperty("error")]
        public NotificationError Error { get; set; }
    }
}
