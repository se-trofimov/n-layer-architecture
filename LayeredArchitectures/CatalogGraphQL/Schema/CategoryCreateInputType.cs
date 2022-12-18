using CatalogGraphQL.Models;
using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class CategoryCreateInputType: InputObjectGraphType
{
    public CategoryCreateInputType()
    {
        Name = "CategoryCreationInput";

        Field<NonNullGraphType<StringGraphType>>("name");
        Field<StringGraphType>("image");
        Field<IntGraphType>("parentCategoryId");
    }
}
