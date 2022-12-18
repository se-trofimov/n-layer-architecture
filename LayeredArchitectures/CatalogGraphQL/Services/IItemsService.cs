using CatalogGraphQL.Models;

namespace CatalogGraphQL.Services;

public interface IItemsService
{
    Task<PagedList<Item>> GetItemsAsync(int categoryId, int pageNum = 1, int pageSize = 10,
        CancellationToken cancellationToken = default);
}