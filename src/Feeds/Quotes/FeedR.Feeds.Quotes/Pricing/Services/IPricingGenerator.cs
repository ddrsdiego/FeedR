namespace FeedR.Feeds.Quotes.Pricing.Services;

using Models;

internal interface IPricingGenerator
{
    IEnumerable<string> GetSymbols();
    IAsyncEnumerable<CurrencyPair> StartAsync();
    Task StopAsync();
}