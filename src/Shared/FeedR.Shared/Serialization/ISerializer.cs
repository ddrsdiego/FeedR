namespace FeedR.Shared.Serialization;

public interface ISerializer
{
    byte[] Serializer<T>(T value);

    T? Deserializer<T>(byte[] value);
}