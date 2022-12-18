using CatalogGraphQL.Models;
using CatalogGraphQL.Services;
using GraphQL;
using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class CategoriesQuery : ObjectGraphType
{
    public CategoriesQuery()
    {
        Name = "CategoryQuery";

        Field<PaginationMetadataType<CategoryType, Category>>("categories")
            .Argument<int>("pageNum")
            .Argument<int>("pageSize")
            .ResolveAsync(async context =>
            {
                var pageNum = context.GetArgument<int>("pageNum");
                var pageCount = context.GetArgument<int>("pageSize");
                var service = context.RequestServices.GetRequiredService<ICategoryService>();
                return await service.GetAsync(pageNum, pageCount);
            });
    }
}
