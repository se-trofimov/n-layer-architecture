using System.Runtime.Serialization;
using AutoMapper;
using CatalogService.Application.Dtos;
using CatalogService.Application.Mappings;
using CatalogService.Application.UseCases.Catalog.Commands;
using CatalogService.Application.UseCases.Items.Commands;
using CatalogService.Domain.Entities;
using NUnit.Framework;

namespace CatalogService.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<ItemsMapping>();
            config.AddProfile<CategoryMapping>();
        });

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(Item), typeof(ItemDto))]
    [TestCase(typeof(CreateItemCommand), typeof(Item))]
    [TestCase(typeof(Category), typeof(CategoryDto))]
    [TestCase(typeof(Category), typeof(CategorySlimDto))]
    [TestCase(typeof(CreateCategoryCommand), typeof(Category))]
    [TestCase(typeof(CreateCategoryCommand), typeof(Category))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}
