namespace FeedR.Feeds.Quotes.Pricing.Models;

public record struct CurrencyPair(string Symbol, decimal Value, long Timestamp);