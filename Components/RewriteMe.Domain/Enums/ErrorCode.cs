using System.Text.Json.Serialization;

namespace RewriteMe.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorCode
    {
        None = 0,

        // Uploaded file not found
        EC1 = 1,

        // File item not found
        EC2 = 2,

        // Language not supported
        EC3 = 3,

        // File not supported
        EC4 = 4,

        // Database error
        EC5 = 5,

        // No available subscription
        EC6 = 6
    }
}
