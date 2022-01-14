namespace FeedR.Feeds.Quotes.Pricing.Services;

using Infra.Options;
using Microsoft.Extensions.Options;
using Models;

public class PricingGenerator : IPricingGenerator
{
    private readonly Random _random = new();
    private readonly AppConfig _appConfig; 
    private readonly ILogger<PricingGenerator> _logger;

    private readonly Dictionary<string, decimal> _currencyPair = new()
    {
        ["EURUSD"] = 1.13M,
        ["EURGBP"] = 0.85M,
        ["EURBLR"] = 6.42M
    };

    private bool _isRunning;

    public PricingGenerator(ILogger<PricingGenerator> logger, IOptions<AppConfig> appConfig)
    {
        if (appConfig == null) throw new ArgumentNullException(nameof(appConfig));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _appConfig = appConfig.Value;
    }

    public async IAsyncEnumerable<CurrencyPair> StartAsync()
    {
        _isRunning = true;
        while (_isRunning)
        {
            foreach (var (symbol, pricing)in _currencyPair)
            {
                if (!_isRunning)
                    yield break;

                var tick = NextTick();
                var newPricing = pricing + tick;

                _currencyPair[symbol] = newPricing;

                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _logger.LogInformation($"Updated pricing for: {symbol}, {pricing:F} -> {newPricing:F} [{tick:F}]");

                var currencyPair = new CurrencyPair(symbol, newPricing, timestamp);
                yield return currencyPair;
                
                await Task.Delay(TimeSpan.FromMilliseconds(_appConfig.DelayGeneratorPricingInMilliseconds));
            }
        }
    }

    public Task StopAsync()
    {
        _isRunning = false;
        return Task.CompletedTask;
    }

    private decimal NextTick()
    {
        var sign = _random.Next(0, 2) == 0 ? -1 : 1;
        var tick = _random.NextDouble() / 20;

        return (decimal)(sign * tick);
    }
}