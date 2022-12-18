using CatalogGraphQL.Models;
using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class CategoryType: ObjectGraphType<Category>
{
    public CategoryType()
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("Category Id");
        Field(x => x.Name).Description("Category Name");
        Field(x => x.Image, nullable: true).Description("Image URL");
        Field<ListGraphType<ItemType>>("items").Resolve(x => x.Source.Items);
        Field<ParentCategoryType>("parentCategory").Resolve(x => x.Source.ParentCategory);
    }
}