using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.UseCases.Catalog.Commands;
using CatalogService.Application.UseCases.Items.Commands;
using CatalogService.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace CatalogService.Application.IntegrationTests.Items.Commands;

using static Testing;

public class DeleteItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new DeleteItemCommand(99, 111);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoList()
    {
        var category = await SendAsync(new CreateCategoryCommand() { Name = "New Category" });

        var command = new CreateItemCommand()
        {
            Price = 1000,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = "Some item",
            Description = "Some description"
        };
        var item = await SendAsync(command);

        await SendAsync(new DeleteItemCommand(item.Id, category.Id));

        var list = await FindAsync<Item>(item.Id);

        list.Should().BeNull();
    }
}
