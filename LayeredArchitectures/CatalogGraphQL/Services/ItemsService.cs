using CatalogGraphQL.Models;
using CatalogGraphQL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatalogGraphQL.Services;

public class ItemsService : IItemsService
{
    private readonly ApplicationDbContext _dbContext;

    public ItemsService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    public async Task<PagedList<Item>> GetItemsAsync(int categoryId, int pageNum = 1, int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var res = await _dbContext.Items
            .Include(x => x.Category)
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Name)
            .ToPagedList(pageNum, pageSize);

        return res;
    }
}
