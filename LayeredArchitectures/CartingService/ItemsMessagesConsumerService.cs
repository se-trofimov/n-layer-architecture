using CartingService.DataAccessLayer;
using CartingService.DataAccessLayer.ValueObjects;
using CartingService.Messaging;
using Messaging.Abstractions;

namespace CartingService;

public class ItemsMessagesConsumerService : IHostedService
{
    private readonly IQueueConsumer _queueConsumer;
    private readonly ICartRepository _cartRepository;

    public ItemsMessagesConsumerService(IQueueConsumer queueConsumer, ICartRepository cartRepository)
    {
        _queueConsumer = queueConsumer ?? throw new ArgumentNullException(nameof(queueConsumer));
        _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(
            () =>
            {
                _queueConsumer.ListenAsync<ItemHasBeenChangedNotification>("items-changed-queue",
                    OnItemHasBeenChangedNotificationReceived, cancellationToken);
            }, cancellationToken);
    }

    private async Task OnItemHasBeenChangedNotificationReceived(ItemHasBeenChangedNotification message)
    {
        await foreach (var cart in _cartRepository.GetCartsWithItemId(message.Id))
        {
            var itemToChange = cart.Items.First(x => x.Id == message.Id);
            itemToChange.Image = new Image(message.Image, itemToChange.Image?.AltText);

            if (!string.IsNullOrWhiteSpace(message.Name))
                itemToChange.Name = message.Name;

            itemToChange.Price = message.Price;
            await _cartRepository.UpdateAsync(cart);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _queueConsumer.Dispose();
        return Task.CompletedTask;
    }
}
