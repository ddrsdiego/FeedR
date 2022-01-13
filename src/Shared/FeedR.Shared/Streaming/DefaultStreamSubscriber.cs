namespace FeedR.Shared.Streaming;

internal sealed class DefaultStreamSubscriber : IStreamSubscriber
{
    public async Task SubscriberAsync<T>(string topic, Func<T, Task> handler) where T : new()
    {
        if (topic == null) throw new ArgumentNullException(nameof(topic));

        if (handler == null) throw new ArgumentNullException(nameof(handler));

        await Task.CompletedTask;
    }
}