namespace CatalogService.Application.Dtos;
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Image { get; set; }
    public CategoryDto? ParentCategory { get; set; }
    public int? ParentCategoryId { get; set; }
}
