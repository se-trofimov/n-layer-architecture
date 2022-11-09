using MediatR;

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