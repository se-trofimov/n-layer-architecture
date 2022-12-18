using CatalogGraphQL.Services;
using GraphQL;
using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class CategoriesQuery : ObjectGraphType
{
    public CategoriesQuery(IServiceProvider provider, ILogger<CategoriesQuery> logger)
    {
        Name = "Query";

        Field<ListGraphType<CategoryType>>("categories")
            .Argument<int>("pageNum")
            .Argument<int>("pageSize")
            .ResolveAsync(async context =>
            {
                try
                {
                    var pageNum = context.GetArgument<int>("pageNum");
                    var pageCount = context.GetArgument<int>("pageSize");
                    var service = context.RequestServices.GetRequiredService<ICategoryService>();
                    return await service.GetAsync(pageNum, pageCount);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Unable perform a query");
                    throw;
                }
            });
    }
}
