namespace FeedR.Shared.Serialization;

public interface ISerializer
{
    string Serializer<T>(T value);

    T? Deserializer<T>(string value);
}