namespace FeedR.Feeds.Quotes.Pricing.Services;

using Requests;
using Shared.Streaming;

internal class PricingBackgroundServices : BackgroundService
{
    private int _runningStatus;
    private readonly IPricingGenerator _pricingGenerator;
    private readonly PricingRequestChannel _requestChannel;
    private readonly IStreamPublisher _publisher;
    private readonly ILogger<PricingBackgroundServices> _logger;

    public PricingBackgroundServices(IPricingGenerator pricingGenerator,
        PricingRequestChannel requestChannel, IStreamPublisher publisher, ILogger<PricingBackgroundServices> logger)
    {
        _pricingGenerator = pricingGenerator ?? throw new ArgumentNullException(nameof(pricingGenerator));
        _requestChannel = requestChannel ?? throw new ArgumentNullException(nameof(requestChannel));
        _publisher = publisher;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Pricing background service has started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            while (await _requestChannel.Channel.Reader.WaitToReadAsync(stoppingToken))
            {
                if (!_requestChannel.Channel.Reader.TryRead(out var request))
                    continue;

                _logger.LogInformation(
                    $"Pricing background service has received the request: '{request.GetType().Name}'.");

                var _ = request switch
                {
                    StartPricing => StartGeneratorAsync(),
                    StopPricing => StopGeneratorAsync(),
                    _ => Task.CompletedTask
                };
            }
        }

        _logger.LogInformation("Pricing background service has stopped.");
    }

    private async Task StartGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
        {
            _logger.LogInformation($"Pricing generator is already running.");
            return;
        }

        await foreach (var currencyPair in _pricingGenerator.StartAsync())
        {
            _logger.LogInformation("Publishing the currency pair...");
            await _publisher.PublishAsync("pricing", currencyPair);
        }
    }

    private async Task StopGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
        {
            _logger.LogInformation($"Pricing generator is not running.");
            return;
        }

        await _pricingGenerator.StopAsync();
    }
}