using CatalogGraphQL.Models;
using CatalogGraphQL.Persistence;
using Microsoft.EntityFrameworkCore;
namespace CatalogGraphQL.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    public async Task<PagedList<Category>> GetAsync(int pageNum = 1, int pageSize= 10, 
        CancellationToken cancellationToken = default)
    {
        var res = await _dbContext.Categories
            .Include(x => x.ParentCategory)
            .Include(x=>x.Items)
            .OrderBy(x=>x.Id)
            .ToPagedList(pageNum, pageSize);

        var lookup = res.ToLookup(x => x.Id);

        foreach (var cat in res)
        {
            if (cat.ParentCategoryId.HasValue)
                cat.ParentCategory = lookup[cat.ParentCategoryId.Value].First();
        }

        return res;
    }
}