using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class ItemUpdateInputType : InputObjectGraphType
{
    public ItemUpdateInputType()
    {
        Name = "ItemUpdateInput";

        Field<NonNullGraphType<IntGraphType>>("id");
        Field<NonNullGraphType<StringGraphType>>("name");
        Field<StringGraphType>("image");
        Field<StringGraphType>("description");
        Field<NonNullGraphType<IntGraphType>>("categoryId");
        Field<NonNullGraphType<DecimalGraphType>>("price");
        Field<NonNullGraphType<IntGraphType>>("amount");
    }
}
