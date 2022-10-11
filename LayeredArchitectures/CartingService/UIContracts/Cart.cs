namespace CartingService.UIContracts
{
    public record Cart(Guid Id, IEnumerable<Item> items);
}
