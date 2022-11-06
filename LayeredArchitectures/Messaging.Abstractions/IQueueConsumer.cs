namespace Messaging.Abstractions;

public interface IQueueConsumer
{
    Task ListenAsync<T>(string destination, Action<T> onMessageReceived, CancellationToken cancellationToken);
}