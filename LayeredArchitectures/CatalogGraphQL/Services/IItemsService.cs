using CatalogGraphQL.Models;

namespace CatalogGraphQL.Services;

public interface IItemsService
{
    Task<PagedList<Item>> GetItemsAsync(int categoryId, int pageNum = 1, int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<Item> CreateItemAsync(Item item, CancellationToken cancellationToken = default);
    Task<Item> UpdateItemAsync(Item item, CancellationToken cancellationToken = default);
    Task DeleteItemAsync(int id, CancellationToken cancellationToken = default);
}