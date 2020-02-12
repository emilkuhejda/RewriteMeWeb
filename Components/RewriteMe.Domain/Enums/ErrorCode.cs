using System.Text.Json.Serialization;

namespace RewriteMe.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorCode
    {
        None = 0,

        // Uploaded file not found
        EC100 = 100,

        // File item not found
        EC101 = 101,

        // Language not supported
        EC200 = 200,

        // File not supported
        EC201 = 201,

        // No available subscription
        EC300 = 300,

        // Database error
        EC400 = 400,

        // Unauthorized
        Unauthorized = 900
    }
}
