namespace FeedR.Shared.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class SystemTextJsonSerializer : ISerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public string Serializer<T>(T value) => JsonSerializer.Serialize(value, Options);

    public T? Deserializer<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);
}