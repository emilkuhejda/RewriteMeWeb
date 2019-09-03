using System.Net;
using Newtonsoft.Json;

namespace RewriteMe.Domain.Notifications
{
    [JsonObject]
    public class NotificationError
    {
        [JsonProperty("code")]
        public HttpStatusCode Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
