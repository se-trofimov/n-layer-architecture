namespace Messaging.Abstractions;

public interface IQueueConsumer
{
    Task ListenAsync<T>(string destination, Func<T, Task> onMessageReceived, CancellationToken cancellationToken);
}