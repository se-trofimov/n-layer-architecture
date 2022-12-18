using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class ItemDeleteInputType : InputObjectGraphType
{
    public ItemDeleteInputType()
    {
        Name = "ItemDeletionInput";
        Field<NonNullGraphType<IntGraphType>>("id");
    }
}
