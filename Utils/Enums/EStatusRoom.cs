using System.Text.Json.Serialization;

namespace LTBACKEND.Utils.Enums
{
   [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EStatusRoom
    {
        AVAILABLE = 1,
        UNAVAILABLE = 0,
    }
}