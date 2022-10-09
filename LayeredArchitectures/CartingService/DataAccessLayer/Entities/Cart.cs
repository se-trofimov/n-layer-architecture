namespace CartingService.DataAccessLayer.Entities
{
    public class Cart
    {
        public string Id { get; set; } 
        public IEnumerable<Item> Items { get; set; } 
    }
}
