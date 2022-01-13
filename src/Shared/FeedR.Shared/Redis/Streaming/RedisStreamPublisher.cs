namespace FeedR.Shared.Redis.Streaming;

using Serialization;
using Shared.Streaming;
using StackExchange.Redis;

internal sealed class RedisStreamPublisher : IStreamPublisher
{
    private readonly ISerializer _serializer;
    private readonly ISubscriber _subscriber;

    public RedisStreamPublisher(IConnectionMultiplexer connectionMultiplexer, ISerializer serializer)
    {
        if (connectionMultiplexer == null) throw new ArgumentNullException(nameof(connectionMultiplexer));

        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _subscriber = connectionMultiplexer.GetSubscriber();
    }

    public ValueTask PublishAsync<T>(string topic, T data) where T : new()
    {
        if (topic == null) throw new ArgumentNullException(nameof(topic));
        
        if (data == null) throw new ArgumentNullException(nameof(data));

        var payload = _serializer.Serializer(data);

        var taskResult = _subscriber.PublishAsync(topic, payload);

        return taskResult.IsCompletedSuccessfully ? new ValueTask() : SlowPublisher(taskResult);
        
        static async ValueTask SlowPublisher(Task<long> taskResult) => await taskResult;
    }
}