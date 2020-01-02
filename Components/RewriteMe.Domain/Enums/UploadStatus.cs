using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RewriteMe.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UploadStatus
    {
        None = 0,
        InProgress = 1,
        Completed = 2,
        Error = 3
    }
}
