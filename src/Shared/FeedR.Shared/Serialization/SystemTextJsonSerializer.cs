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

    public byte[] Serializer<T>(T value) => JsonSerializer.SerializeToUtf8Bytes(value, Options);

    public T? Deserializer<T>(byte[] value) => JsonSerializer.Deserialize<T>(value, Options);
}