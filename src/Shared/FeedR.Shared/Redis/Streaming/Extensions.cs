namespace FeedR.Shared.Redis.Streaming;

using Microsoft.Extensions.DependencyInjection;
using Shared.Streaming;

public static class Extensions
{
    public static IServiceCollection AddRedisStreaming(this IServiceCollection services)
        => services
            .AddSingleton<IStreamPublisher, RedisStreamPublisher>()
            .AddSingleton<IStreamSubscriber, RedisStreamSubscriber>();
}