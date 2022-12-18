using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class PaginationMetadataType<T, K> : ObjectGraphType<PagedList<K>>
where T: ObjectGraphType<K>
{
    public PaginationMetadataType()
    {
        Field(x => x.PageSize);
        Field(x => x.TotalPages);
        Field(x => x.TotalCount);
        Field(x => x.CurrentPage);
        Field(x => x.HasNext);
        Field(x => x.HasPrevious);
        Field<ListGraphType<T>>("items").Resolve(x => x.Source);
    }
}
