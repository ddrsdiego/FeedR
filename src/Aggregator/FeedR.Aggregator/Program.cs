using FeedR.Aggregator.Infra.Options;
using FeedR.Aggregator.Services;
using FeedR.Aggregator.Services.Channels;
using FeedR.Shared.Redis;
using FeedR.Shared.Redis.Streaming;
using FeedR.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddOptions(builder.Configuration)
    .AddHostedService<PricingStreamBackgroundService>()
    .AddHostedService<ConsumerChannelCurrencyPairsService>()
    .AddSerialization()
    .AddChannels()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming();


var app = builder.Build();

app.MapGet("/", () => "FeedR Aggregator");

app.Run();