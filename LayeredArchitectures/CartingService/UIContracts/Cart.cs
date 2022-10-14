namespace CartingService.UIContracts
{
    public record NewCart(Guid Id);
    public record Cart(Guid Id, IEnumerable<Item> items);
}
