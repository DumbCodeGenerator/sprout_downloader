using System.Text.Json.Serialization;

namespace Sprout_Downloader.Json
{
    [JsonSerializable(typeof(SproutData))]
    internal partial class JsonContext : JsonSerializerContext
    {
    }
}
