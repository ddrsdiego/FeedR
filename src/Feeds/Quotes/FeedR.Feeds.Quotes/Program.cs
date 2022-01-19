using FeedR.Feeds.Quotes.Infra.Options;
using FeedR.Feeds.Quotes.Pricing.Requests;
using FeedR.Feeds.Quotes.Pricing.Services;
using FeedR.Shared.Redis;
using FeedR.Shared.Redis.Streaming;
using FeedR.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddOptions(builder.Configuration)
    .AddSerialization()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddSingleton<PricingRequestChannel>()
    .AddSingleton<IPricingGenerator, PricingGenerator>()
    .AddHostedService<PricingBackgroundServices>()
    .AddGrpc();

var app = builder.Build();

app.MapGrpcService<PricingGrpcService>();
app.MapGet("/", () => "FeedR Quotes feed");

app.MapPost("/pricing/start",
    async (PricingRequestChannel channel) =>
    {
        await channel.Channel.Writer.WriteAsync(new StartPricing());
        return Results.Ok();
    });

app.MapPost("/pricing/stop",
    async (PricingRequestChannel channel) =>
    {
        await channel.Channel.Writer.WriteAsync(new StopPricing());
        return Results.Ok();
    });

app.Run();