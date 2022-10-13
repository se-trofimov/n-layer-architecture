namespace CatalogService.Application.Dtos;
public class ItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public CategorySlimDto Category { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
}
