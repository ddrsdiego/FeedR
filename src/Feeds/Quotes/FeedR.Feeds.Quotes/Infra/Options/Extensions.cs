namespace FeedR.Feeds.Quotes.Infra.Options;

public static class Extensions
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppConfig>(configuration.GetSection(nameof(AppConfig)));
        return services;
    }
}