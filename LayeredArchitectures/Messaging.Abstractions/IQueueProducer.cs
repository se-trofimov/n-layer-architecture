namespace Messaging.Abstractions;

public interface IQueueProducer<TMessage>
{
    Task Send(TMessage message, string destination);
}