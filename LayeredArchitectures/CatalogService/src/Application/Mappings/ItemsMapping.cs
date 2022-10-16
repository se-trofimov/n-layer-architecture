using AutoMapper;
using CatalogService.Application.Dtos;
using CatalogService.Application.UseCases.Items.Commands;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.Mappings;
public class ItemsMapping: Profile
{
    public ItemsMapping()
    {
        CreateMap<Item, ItemDto>();
        CreateMap<CreateItemCommand, Item>();
        CreateMap<ChangeItemCommand, Item>();
    }
}
