namespace FeedR.Shared.Streaming;

public interface IStreamSubscriber
{
    Task SubscriberAsync<T>(string topic, Func<T, Task> handler) where T : new();
}