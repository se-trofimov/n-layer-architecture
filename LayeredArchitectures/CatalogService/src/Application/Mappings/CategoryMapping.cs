using CatalogService.Application.Common.Mappings;
using CatalogService.Application.Dtos;
using CatalogService.Application.UseCases.Catalog.Commands;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.Mappings;
public class CategoryMapping: MappingProfile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<ChangeCategoryCommand, ChangeCategoryWithIdCommand>();
        CreateMap<CreateCategoryCommand, Category>();
        CreateMap<ChangeCategoryWithIdCommand, Category>()
            .ForMember(x=>x.Id, expression => expression.Ignore());
    }
}
