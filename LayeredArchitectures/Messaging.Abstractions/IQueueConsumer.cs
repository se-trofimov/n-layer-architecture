namespace Messaging.Abstractions;

public interface IQueueConsumer: IDisposable
{
    Task ListenAsync<T>(string destination, Func<T, Task> onMessageReceived, CancellationToken cancellationToken);
}