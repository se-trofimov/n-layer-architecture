namespace CatalogGraphQL.Models;

public class CategoryCreateInput
{
    public string Name { get; set; }
    public string? Image { get; set; }
    public int? ParentCategoryId { get; set; }
}
