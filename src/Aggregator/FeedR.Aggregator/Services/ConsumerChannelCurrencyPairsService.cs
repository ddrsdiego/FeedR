namespace FeedR.Aggregator.Services;

using System.Collections;
using System.Threading.Channels;
using Infra.Options;
using Microsoft.Extensions.Options;

internal sealed class ConsumerChannelCurrencyPairsService : BackgroundService
{
    private readonly Channel<CurrencyPair> _channel;
    private readonly ILogger<ConsumerChannelCurrencyPairsService> _logger;
    private readonly AppConfig _appConfig;

    public ConsumerChannelCurrencyPairsService(ILogger<ConsumerChannelCurrencyPairsService> logger,
        IOptions<AppConfig> appConfig, Channel<CurrencyPair> channel)
    {
        if (appConfig == null) throw new ArgumentNullException(nameof(appConfig));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _appConfig = appConfig.Value;
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            while (await _channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var batchCurrencyPair = new List<CurrencyPair>(_appConfig.BatchCurrencyPair);

                while (TryRead(batchCurrencyPair, out var currencyPair))
                {
                    batchCurrencyPair.Add(currencyPair);
                }

                await Task.Delay(1_000, stoppingToken);
                _logger.LogInformation($"{batchCurrencyPair.Count} messages processed successfully...");

                batchCurrencyPair.Clear();
                batchCurrencyPair = null;
            }
        }
    }

    private bool TryRead(ICollection currencyPairs, out CurrencyPair currencyPair) =>
        _channel.Reader.TryRead(out currencyPair) && currencyPairs.Count < _appConfig.BatchCurrencyPair;
}