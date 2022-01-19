namespace FeedR.Feeds.Quotes.Pricing.Services;

using Grpc.Core;

internal sealed class PricingGrpcService : PricingFeed.PricingFeedBase
{
    private readonly IPricingGenerator _pricingGenerator;

    public PricingGrpcService(IPricingGenerator pricingGenerator)
    {
        _pricingGenerator = pricingGenerator;
    }

    public override Task<GetSymbolsResponse> GetSymbols(GetSymbolsRequest request, ServerCallContext context)
        => Task.FromResult(new GetSymbolsResponse
        {
            Symbols = { _pricingGenerator.GetSymbols() }
        });
}