using System.Text.Json.Serialization;

namespace RewriteMe.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StorageSetting
    {
        Disk = 0,
        Database = 1,
        Azure = 2
    }
}
