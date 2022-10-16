using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.UseCases.Catalog.Commands;
using CatalogService.Application.UseCases.Items.Commands;
using CatalogService.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace CatalogService.Application.IntegrationTests.Categories.Commands;

using static Testing;

public class DeleteCategoryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteCategoryCommand(99);

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteCategory()
    {
        var category = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Category"
        });


        await SendAsync(new DeleteCategoryCommand(category.Id));

        var item = await FindAsync<Category>(category.Id);

        item.Should().BeNull();
    }

    [Test]
    public async Task ShouldDeleteCategoryWithParent()
    {
        var categoryParent = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Category Parent"
        });

        var category = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Category",
            ParentCategoryId = categoryParent.Id
        });


        await SendAsync(new DeleteCategoryCommand(category.Id));

        var item = await FindAsync<Category>(category.Id);

        item.Should().BeNull();
    }

    [Test]
    public async Task ShouldDeleteCategoryWithParentAndItems()
    {
        var categoryParent = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Category Parent"
        });

        var category = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Category",
            ParentCategoryId = categoryParent.Id
        });

        var item1 = await SendAsync(new CreateItemCommand()
        {
            Name = "New Item 1",
            CategoryId = category.Id,
            Price = 100,
            Description = "Some description",
            Amount = 10,
            Image = "Some image.png"
        });

        var item2 = await SendAsync(new CreateItemCommand()
        {
            Name = "New Item 1",
            CategoryId = category.Id,
            Price = 100,
            Description = "Some description",
            Amount = 10,
            Image = "Some image.png"
        });


        await SendAsync(new DeleteCategoryCommand(category.Id));

        (await FindAsync<Category>(category.Id)).Should().BeNull();
        (await FindAsync<Item>(category.Id)).Should().BeNull();
        (await FindAsync<Item>(category.Id)).Should().BeNull();
    }
}
