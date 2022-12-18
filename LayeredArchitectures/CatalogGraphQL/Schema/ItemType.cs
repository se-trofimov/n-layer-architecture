using CatalogGraphQL.Models;
using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class ItemType : ObjectGraphType<Item>
{
    public ItemType()
    {
        Field(i => i.Id);
        Field(i => i.Name);
        Field(i => i.Description, nullable: true);
        Field(i => i.Price);
        Field(i => i.Amount);
        Field(i => i.Image, nullable: true);
        Field<ParentCategoryType>("category").Resolve(x => x.Source.Category);
    }
}