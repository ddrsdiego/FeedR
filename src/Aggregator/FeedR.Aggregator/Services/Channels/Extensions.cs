namespace FeedR.Aggregator.Services.Channels;

using System.Threading.Channels;

public static class Extensions
{
    public static IServiceCollection AddChannels(this IServiceCollection services)
    {
        services
            .AddSingleton(Channel.CreateBounded<CurrencyPair>(new BoundedChannelOptions(1_000)
                { SingleReader = true }));

        return services;
    }
}