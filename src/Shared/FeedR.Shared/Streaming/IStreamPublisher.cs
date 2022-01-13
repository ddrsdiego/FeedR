namespace FeedR.Shared.Streaming;

public interface IStreamPublisher
{
    ValueTask PublishAsync<T>(string topic, T data) where T : new();
}