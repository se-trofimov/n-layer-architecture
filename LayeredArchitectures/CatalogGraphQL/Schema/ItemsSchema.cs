namespace CatalogGraphQL.Schema;

public class ItemsSchema : GraphQL.Types.Schema
{
    public ItemsSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<ItemsQuery>();
        Mutation = serviceProvider.GetRequiredService<ItemsMutation>();
    }
}
