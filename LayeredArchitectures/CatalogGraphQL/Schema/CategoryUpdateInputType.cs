using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class CategoryUpdateInputType : InputObjectGraphType
{
    public CategoryUpdateInputType()
    {
        Name = "CategoryUpdateInput";

        Field<NonNullGraphType<IntGraphType>>("id");
        Field<NonNullGraphType<StringGraphType>>("name");
        Field<StringGraphType>("image");
        Field<IntGraphType>("parentCategoryId");
    }
}
