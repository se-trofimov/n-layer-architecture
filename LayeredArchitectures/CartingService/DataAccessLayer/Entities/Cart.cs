namespace CartingService.DataAccessLayer.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public IEnumerable<Item> Items { get; set; } = Enumerable.Empty<Item>();

        public void AddItem(Item item)
        {
            if (Items.FirstOrDefault(x => x.Id == item.Id) is { } existingItem)
            {
                existingItem.Quantity += item.Quantity;
                existingItem.Price = item.Price;
            }
            else
            {
                var items = Items.ToList();
                items.Add(item);
                Items = items;
            }
        }
    }
}
