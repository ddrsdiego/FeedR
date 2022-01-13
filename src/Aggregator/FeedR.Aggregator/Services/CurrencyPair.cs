namespace FeedR.Aggregator.Services;

public record struct CurrencyPair(string Symbol, decimal Value, long Timestamp);