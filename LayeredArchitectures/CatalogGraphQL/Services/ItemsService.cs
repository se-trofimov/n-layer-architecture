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

    public async Task<Item> CreateItemAsync(Item item, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == item.CategoryId, cancellationToken: cancellationToken);
        if (category is null)
            throw new Exception($"Category with id {item.CategoryId} not found");
        _dbContext.Items.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task<Item> UpdateItemAsync(Item item, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == item.Id, cancellationToken: cancellationToken);
        if (exists is null)
            throw new Exception($"Item with id {item.Id} not found");

        var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == item.CategoryId, cancellationToken: cancellationToken);
        if (category is null)
            throw new Exception($"Category with id {item.CategoryId} not found");

        _dbContext.Items.Update(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task DeleteItemAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        if (exists is null)
            throw new Exception($"Item with id {id} not found");

        _dbContext.Items.Remove(exists);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
