namespace FeedR.Shared.Streaming;

using Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddStreaming(this IServiceCollection services)
    {
        services
            .AddSingleton<IStreamPublisher, DefaultStreamPublisher>()
            .AddSingleton<IStreamSubscriber, DefaultStreamSubscriber>();
        return services;
    }
}