using System.Collections.Immutable;

namespace CartingService.DataAccessLayer.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public ImmutableList<Item> Items { get; set; } =  ImmutableList<Item>.Empty;

        public void AddItem(Item item)
        {
            if (Items.FirstOrDefault(x => x.Id == item.Id) is { } existingItem)
            {
                existingItem.Quantity += item.Quantity;
                existingItem.Price = item.Price;
            }
            else
            {
                Items = Items.Add(item);
            }
        }
    }
}
