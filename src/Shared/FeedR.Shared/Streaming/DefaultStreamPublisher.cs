namespace FeedR.Shared.Streaming;

internal sealed class DefaultStreamPublisher : IStreamPublisher
{
    public DefaultStreamPublisher()
    {
        
    }
    
    public async ValueTask PublishAsync<T>(string topic, T data) where T : new()
    {
        await Task.CompletedTask;
    }
}