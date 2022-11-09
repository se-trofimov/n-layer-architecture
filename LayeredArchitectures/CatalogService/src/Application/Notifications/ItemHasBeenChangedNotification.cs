using MediatR;
using Messaging.Abstractions;

namespace CatalogService.Application.Notifications;
public class ItemHasBeenChangedNotification : INotification
{
    public ItemHasBeenChangedNotification(int id, string name, string? image, decimal price)
    {
        Id = id;
        Name = name;
        Image = image;
        Price = price;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
}

public class ItemHasBeenChangedNotificationHandler : INotificationHandler<ItemHasBeenChangedNotification>
{
    private readonly IQueueProducer<ItemHasBeenChangedNotification> _producer;

    public ItemHasBeenChangedNotificationHandler(IQueueProducer<ItemHasBeenChangedNotification> producer)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
    }
    public async Task Handle(ItemHasBeenChangedNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            await _producer.Send(notification, "items-changed-queue");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        
        }
    }
}
