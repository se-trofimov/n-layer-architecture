using CatalogGraphQL.Models;
using CatalogGraphQL.Services;
using GraphQL;
using GraphQL.Types;

namespace CatalogGraphQL.Schema;

public sealed class ItemsMutation : ObjectGraphType
{
    public ItemsMutation()
    {
        Name = "Mutation";

        Field<ItemType>("createItem")
            .Argument<NonNullGraphType<ItemCreateInputType>>("item")
            .ResolveAsync(async context =>
            {
                var service = context.RequestServices.GetRequiredService<IItemsService>();
                var argument = context.GetArgument<Item>("item");
                return await service.CreateItemAsync(argument);
            });

        Field<ItemType>("updateItem")
            .Argument<NonNullGraphType<ItemUpdateInputType>>("item")
            .ResolveAsync(async context =>
            {
                var service = context.RequestServices.GetRequiredService<IItemsService>();
                var argument = context.GetArgument<Item>("item");
                return await service.UpdateItemAsync(argument);
            });

        Field<ItemType>("deleteItem")
            .Argument<NonNullGraphType<ItemDeleteInputType>>("item")
            .ResolveAsync(async context =>
            {
                var service = context.RequestServices.GetRequiredService<IItemsService>();
                var argument = context.GetArgument<Item>("item");
                await service.DeleteItemAsync(argument.Id);
                return null;
            });
    }
}
