namespace CartingService.UIContracts
{
    public record Item(int Id, string Name, Image? Image, decimal Price, int Quantity);
}
