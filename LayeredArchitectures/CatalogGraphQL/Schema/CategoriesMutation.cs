using CatalogGraphQL.Models;
using CatalogGraphQL.Services;
using GraphQL;
using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class CategoriesMutation : ObjectGraphType
{
    public CategoriesMutation()
    {
        Name = "Mutation";

        Field<CategoryType>("createCategory")
            .Argument<NonNullGraphType<CategoryCreateInputType>>("category")
            .ResolveAsync(async context =>
            {
                var service = context.RequestServices.GetRequiredService<ICategoryService>();
                var argument = context.GetArgument<Category>("category");
                return await service.CreateCategoryAsync(argument);
            });

        Field<CategoryType>("updateCategory")
            .Argument<NonNullGraphType<CategoryUpdateInputType>>("category")
            .ResolveAsync(async context =>
            {
                var service = context.RequestServices.GetRequiredService<ICategoryService>();
                var argument = context.GetArgument<Category>("category");
                return await service.UpdateCategoryAsync(argument);
            });


        Field<CategoryType>("deleteCategory")
            .Argument<NonNullGraphType<CategoryDeleteInputType>>("category")
            .ResolveAsync(async context =>
            {
                var service = context.RequestServices.GetRequiredService<ICategoryService>();
                var argument = context.GetArgument<Category>("category");
                await service.DeleteCategoryAsync(argument.Id);
                return null;
            });
    }
}
