using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.UseCases.Catalog.Commands;
using CatalogService.Application.UseCases.Items.Commands;
using CatalogService.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace CatalogService.Application.IntegrationTests.Categories.Commands;

using static Testing;

public class UpdateCategoryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new ChangeCategoryCommand { Id = 99, Name = "New Title" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldUpdateCategory()
    {
         
        var categoryParent = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Category Parent"
        });

        var category = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Category"
        });

        var newItem = await SendAsync(new CreateItemCommand()
        {
            CategoryId = category.Id,
            Name = "New Item",
            Price = 100,
            Amount = 10
        });

        var command = new ChangeCategoryCommand
        {
            Id = category.Id,
            Image = "NewImage.png",
            Name = "NewName",
            ParentCategoryId = categoryParent.Id
        };

        await SendAsync(command);

        var c = await FindAsync<Category>(category.Id);
        c.Should().NotBeNull();
        c.Items.Count.Should().Be(1);
        c.Name.Should().Be(command.Name);
        c.Image.Should().Be(command.Image);
        c.ParentCategory.Id.Should().Be(categoryParent.Id);
    }
}
