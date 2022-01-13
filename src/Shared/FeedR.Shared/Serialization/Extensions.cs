namespace FeedR.Shared.Serialization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddSerialization(this IServiceCollection services) =>
        services.AddSingleton<ISerializer, SystemTextJsonSerializer>();
}