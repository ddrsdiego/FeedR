namespace FeedR.Feeds.Quotes.Pricing.Services;

using System.Threading.Channels;
using Requests;

internal class PricingRequestChannel
{
    public readonly Channel<IPricingRequest> Channel =
        System.Threading.Channels.Channel.CreateUnbounded<IPricingRequest>();
}