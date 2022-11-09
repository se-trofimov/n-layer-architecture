using MediatR;
using Messaging.Abstractions;

namespace CatalogService.Application.Notifications;

public class ItemHasBeenChangedNotificationHandler : INotificationHandler<ItemHasBeenChangedNotification>
{
    private readonly IQueueProducer<ItemHasBeenChangedNotification> _producer;

    public ItemHasBeenChangedNotificationHandler(IQueueProducer<ItemHasBeenChangedNotification> producer)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
    }
    public async Task Handle(ItemHasBeenChangedNotification notification, CancellationToken cancellationToken)
    {
        await _producer.Send(notification, "items-changed-queue");
    }
}