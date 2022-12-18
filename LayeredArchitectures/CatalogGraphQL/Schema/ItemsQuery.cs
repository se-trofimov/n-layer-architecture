using CatalogGraphQL.Models;
using CatalogGraphQL.Services;
using GraphQL;
using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class ItemsQuery : ObjectGraphType
{
    public ItemsQuery()
    {
        Name = "Query";

        Field<PaginationMetadataType<ItemType, Item>>("items")
            .Argument<int>("pageNum")
            .Argument<int>("pageSize")
            .Argument<int>("categoryId")
            .ResolveAsync(async context =>
            {
                var pageNum = context.GetArgument<int>("pageNum");
                var pageCount = context.GetArgument<int>("pageSize");
                var categoryId = context.GetArgument<int>("categoryId");
                var service = context.RequestServices.GetRequiredService<IItemsService>();
                return await service.GetItemsAsync(categoryId, pageNum, pageCount);
            });
    }
}
