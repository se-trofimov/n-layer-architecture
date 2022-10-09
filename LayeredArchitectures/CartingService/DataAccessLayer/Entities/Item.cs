using CartingService.DataAccessLayer.ValueObjects;

namespace CartingService.DataAccessLayer.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Image? Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
