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

    public async Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (category.ParentCategoryId.HasValue)
        {
            var parent = await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == category.ParentCategoryId, cancellationToken: cancellationToken);

            if (parent is null)
                throw new InvalidOperationException($"Parent Category with Id {category.ParentCategoryId} not found");
        }

        _dbContext.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        var existingCategory = await _dbContext.Categories
            .Include(x=>x.Items)
            .FirstOrDefaultAsync(x => x.Id == category.Id, cancellationToken: cancellationToken);

        if (existingCategory is null)
            throw new InvalidOperationException($"Category with Id {category.Id} not found");
        
        if (category.ParentCategoryId.HasValue)
        {
            var parent = await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == category.ParentCategoryId, cancellationToken: cancellationToken);

            if (parent is null)
                throw new InvalidOperationException($"Parent Category with Id {category.ParentCategoryId} not found");
        }

        existingCategory.Name = category.Name;
        existingCategory.Image = category.Image;
        existingCategory.ParentCategoryId = category.ParentCategoryId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return existingCategory;
    }

    public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var existingCategory = await _dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        if (existingCategory is null)
            throw new InvalidOperationException($"Category with Id {id} not found");
        _dbContext.Categories.Remove(existingCategory);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}