namespace FeedR.Feeds.Quotes.Pricing.Services;

using System.Collections.Concurrent;
using Grpc.Core;
using Models;

internal sealed class PricingGrpcService : PricingFeed.PricingFeedBase
{
    private readonly IPricingGenerator _pricingGenerator;
    private readonly BlockingCollection<CurrencyPair> _currencyPairs = new();

    public PricingGrpcService(IPricingGenerator pricingGenerator)
    {
        _pricingGenerator = pricingGenerator;
    }

    public override Task<GetSymbolsResponse> GetSymbols(GetSymbolsRequest request, ServerCallContext context)
        => Task.FromResult(new GetSymbolsResponse
        {
            Symbols = { _pricingGenerator.GetSymbols() }
        });

    public override async Task SubscriberPricing(PricingRequest request,
        IServerStreamWriter<PricingResponse> responseStream,
        ServerCallContext context)
    {
        _pricingGenerator.PricingUpdated += OnPricingUpdated;

        while (!context.CancellationToken.IsCancellationRequested)
        {
            if (!_currencyPairs.TryTake(out var currencyPair))
                continue;

            if (!string.IsNullOrEmpty(request.Symbol) && request.Symbol != currencyPair.Symbol)
                continue;

            await responseStream.WriteAsync(new PricingResponse
            {
                Symbol = currencyPair.Symbol,
                Value = (int) (100 * currencyPair.Value),
                Timestamp = currencyPair.Timestamp
            });
        }

        _pricingGenerator.PricingUpdated -= OnPricingUpdated;

        void OnPricingUpdated(object? sender, CurrencyPair currencyPair) => _currencyPairs.TryAdd(currencyPair);
    }
}