using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.UseCases.Catalog.Commands;
using CatalogService.Application.UseCases.Items.Commands;
using FluentAssertions;
using NUnit.Framework;

namespace CatalogService.Application.IntegrationTests.Items.Commands;

using static Testing;

public class CreateItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateItemCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireExistingCategory()
    {
        var command = new CreateItemCommand()
        {
            Name = "Some Item",
            Image = "Image",
            Price = 1000,
            CategoryId = 999,
            Description = "Some description",
            Amount = 10
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldCreateItem()
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

        item.Should().NotBeNull();
        item.Name.Should().Be(command.Name);
        item.Image.Should().Be(command.Image);
        item.Category.Id.Should().Be(command.CategoryId);
        item.Amount.Should().Be(command.Amount);
        item.Price.Should().Be(command.Price);
        item.Description.Should().Be(command.Description);
    }
}
