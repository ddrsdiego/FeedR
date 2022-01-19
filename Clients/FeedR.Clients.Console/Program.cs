using FeedR.Clients.Console;
using Grpc.Net.Client;

Console.WriteLine("FeedR console client");

using var channel = GrpcChannel.ForAddress("http://localhost:5041");
var client = new PricingFeed.PricingFeedClient(channel);

Console.WriteLine("Press any key to get symbols...");
Console.ReadKey();

var symbolsResponse = await client.GetSymbolsAsync(new GetSymbolsRequest());
foreach (var symbol in symbolsResponse.Symbols)
{
    Console.WriteLine(symbol);
}