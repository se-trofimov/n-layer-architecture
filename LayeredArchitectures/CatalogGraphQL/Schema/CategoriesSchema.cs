namespace CatalogGraphQL.Schema;

public class CategoriesSchema: GraphQL.Types.Schema
{
    public CategoriesSchema(IServiceProvider serviceProvider): base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<CategoriesQuery>();
        Mutation = serviceProvider.GetRequiredService<CategoriesMutation>();
    }
}