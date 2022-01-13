namespace FeedR.Shared.Redis.Streaming;

using Serialization;
using Shared.Streaming;
using StackExchange.Redis;

internal sealed class RedisStreamSubscriber : IStreamSubscriber
{
    private readonly ISubscriber _subscriber;
    private readonly ISerializer _serializer;

    public RedisStreamSubscriber(IConnectionMultiplexer connectionMultiplexer, ISerializer serializer)
    {
        _serializer = serializer;
        _subscriber = connectionMultiplexer.GetSubscriber();
    }

    public Task SubscriberAsync<T>(string topic, Func<T, Task> handler) where T : new()
        => _subscriber.SubscribeAsync(topic, (_, data) =>
        {
            try
            {
                var payload = _serializer.Deserializer<T>(data);
                if (payload is null) return;

                handler(payload);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
}