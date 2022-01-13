namespace FeedR.Feeds.Quotes.Pricing.Services;

using Models;

internal interface IPricingGenerator
{
    IAsyncEnumerable<CurrencyPair> StartAsync();
    Task StopAsync();
}