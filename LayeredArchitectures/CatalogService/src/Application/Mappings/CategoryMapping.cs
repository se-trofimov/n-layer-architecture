using CatalogService.Application.Common.Mappings;
using CatalogService.Application.Dtos;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.Mappings;
public class CategoryMapping: MappingProfile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryDto>();
    }
}
