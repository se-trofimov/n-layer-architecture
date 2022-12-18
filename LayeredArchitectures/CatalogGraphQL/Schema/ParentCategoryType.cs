using CatalogGraphQL.Models;
using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class ParentCategoryType : ObjectGraphType<Category>
{
    public ParentCategoryType()
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("Category Id");
        Field(x => x.Name).Description("Category Name");
    }
}