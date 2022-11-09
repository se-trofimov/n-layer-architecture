using CartingService.Messaging;
using Messaging.Abstractions;

namespace CartingService;

public class ItemsMessagesConsumerService: IHostedService
{
    private readonly IQueueConsumer _queueConsumer;

    public ItemsMessagesConsumerService(IQueueConsumer queueConsumer)
    {
        _queueConsumer = queueConsumer ?? throw new ArgumentNullException(nameof(queueConsumer));
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _queueConsumer.ListenAsync<ItemHasBeenChangedNotification>("items-changed-queue", OnItemHasBeenChangedNotificationReceived, cancellationToken);
    }

    private Task OnItemHasBeenChangedNotificationReceived(ItemHasBeenChangedNotification message)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
