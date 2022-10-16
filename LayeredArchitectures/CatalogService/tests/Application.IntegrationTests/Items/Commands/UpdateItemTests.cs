using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.UseCases.Catalog.Commands;
using CatalogService.Application.UseCases.Items.Commands;
using CatalogService.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace CatalogService.Application.IntegrationTests.Items.Commands;

using static Testing;

public class UpdateTodoListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new ChangeItemCommand()
        {
            Id = 99,
            Name = "New Title",
            Amount = 100,
            Price = 10
        };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireTitle()
    {
        var category = await SendAsync(new CreateCategoryCommand() { Name = "New Category" });
        var item = await SendAsync(new CreateItemCommand()
        {
            Price = 1000,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = "Some item",
            Description = "Some description"
        });

        var command = new ChangeItemCommand()
        {
            Price = 1000,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = "",
            Description = "Some description"
        };

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name")))
                .And.Errors["Name"].Should().Contain("'Name' must not be empty.");
    }

    [Test]
    public async Task ShouldRequireTitleShorterThan50()
    {
        var category = await SendAsync(new CreateCategoryCommand() { Name = "New Category" });
        var item = await SendAsync(new CreateItemCommand()
        {
            Price = 1000,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = "Some item",
            Description = "Some description"
        });

        var command = new ChangeItemCommand()
        {
            Price = 1000,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = new string('.', 51),
            Description = "Some description"
        };

        (await FluentActions.Invoking(() =>
                    SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name")))
            .And.Errors["Name"].Should().Contain("The length of 'Name' must be 50 characters or fewer. You entered 51 characters.");
    }


    [Test]
    public async Task ShouldRequirePrice()
    {
        var category = await SendAsync(new CreateCategoryCommand() { Name = "New Category" });
        var item = await SendAsync(new CreateItemCommand()
        {
            Price = 1000,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = "Some item",
            Description = "Some description"
        });

        var command = new ChangeItemCommand()
        {
            Price = 0,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = "New Name",
            Description = "Some description"
        };

        (await FluentActions.Invoking(() =>
                    SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Price")))
            .And.Errors["Price"].Should().Contain("'Price' must be greater than '0'.");
    }

    [Test]
    public async Task ShouldRequireAmount()
    {
        var category = await SendAsync(new CreateCategoryCommand() { Name = "New Category" });
        var item = await SendAsync(new CreateItemCommand()
        {
            Price = 1000,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = "Some item",
            Description = "Some description"
        });

        var command = new ChangeItemCommand()
        {
            Price = 10,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 0,
            Name = "New Name",
            Description = "Some description"
        };

        (await FluentActions.Invoking(() =>
                    SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Amount")))
            .And.Errors["Amount"].Should().Contain("'Amount' must be greater than '0'.");
    }

    [Test]
    public async Task ShouldUpdateItem()
    {
        var category = await SendAsync(new CreateCategoryCommand() { Name = "New Category" });
        var item = await SendAsync(new CreateItemCommand()
        {
            Price = 1000,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = "Some item",
            Description = "Some description"
        });

        var command = new ChangeItemCommand()
        {
            Id = item.Id,
            Price = 10,
            Image = "Some image",
            CategoryId = category.Id,
            Amount = 10,
            Name = "New Name",
            Description = "Some description"
        };

        await SendAsync(command);
        var found = await FindAsync<Item>(item.Id);

        found.Should().NotBeNull();
        found.Name.Should().Be(command.Name);
        found.Image.Should().Be(command.Image);
        found.Category.Id.Should().Be(command.CategoryId);
        found.Amount.Should().Be(command.Amount);
        found.Price.Should().Be(command.Price);
        found.Description.Should().Be(command.Description);
    }
}
