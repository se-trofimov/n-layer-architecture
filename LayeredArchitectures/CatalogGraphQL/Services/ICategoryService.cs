using CatalogGraphQL.Models;

namespace CatalogGraphQL.Services;

public interface ICategoryService
{
    Task<PagedList<Category>> GetAsync(int pageNum = 1, int pageSize= 10, 
        CancellationToken cancellationToken = default);
    Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken = default);
    Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
    Task DeleteCategoryAsync(int argumentId, CancellationToken cancellationToken = default);
}