using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class CategoryDeleteInputType : InputObjectGraphType
{
    public CategoryDeleteInputType()
    {
        Name = "CategoryDeletionInput";
        Field<NonNullGraphType<IntGraphType>>("id");
    }
}
