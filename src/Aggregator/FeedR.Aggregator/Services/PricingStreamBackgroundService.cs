namespace FeedR.Aggregator.Services;

using System.Threading.Channels;
using Shared.Streaming;

internal sealed class PricingStreamBackgroundService : BackgroundService
{
    private readonly Channel<CurrencyPair> _channel;
    private readonly IStreamSubscriber _streamSubscriber;
    private readonly ILogger<PricingStreamBackgroundService> _logger;

    public PricingStreamBackgroundService(IStreamSubscriber streamSubscriber, Channel<CurrencyPair> channel,
        ILogger<PricingStreamBackgroundService> logger)
    {
        _logger = logger;
        _channel = channel;
        _streamSubscriber = streamSubscriber;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _streamSubscriber.SubscriberAsync<CurrencyPair>("pricing", currencyPair =>
        {
            var (symbol, value, timestamp) = currencyPair;

            _logger.LogInformation(
                $"Princing '{symbol}' = {value:F}, {timestamp}");

            var writeTask = _channel.Writer.WriteAsync(currencyPair, stoppingToken);
            return writeTask.IsCompletedSuccessfully ? Task.CompletedTask : SlowWrite(writeTask);

            static async Task SlowWrite(ValueTask task) => await task;
        });
    }

    public override void Dispose()
    {
        _channel.Writer.TryComplete();
        base.Dispose();
    }
}