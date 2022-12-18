using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class ItemCreateInputType : InputObjectGraphType
{
    public ItemCreateInputType()
    {
        Name = "ItemCreationInput";

        Field<NonNullGraphType<StringGraphType>>("name");
        Field<StringGraphType>("image");
        Field<StringGraphType>("description");
        Field<NonNullGraphType<IntGraphType>>("categoryId");
        Field<NonNullGraphType<DecimalGraphType>>("price");
        Field<NonNullGraphType<IntGraphType>>("amount");
    }
}
