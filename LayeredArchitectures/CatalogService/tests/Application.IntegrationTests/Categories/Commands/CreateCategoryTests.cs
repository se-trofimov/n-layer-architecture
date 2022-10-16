using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.UseCases.Catalog.Commands;
using FluentAssertions;
using NUnit.Framework;

namespace CatalogService.Application.IntegrationTests.Categories.Commands;

using static Testing;

public class CreateCategoryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateCategoryCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateCategory()
    {
        var category = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Category"
        });

        category.Should().NotBeNull();

        category.Name.Should().Be(category.Name);
        category.ParentCategory.Should().BeNull();
        category.Image.Should().BeNull();
    }

    [Test]
    public async Task ShouldCreateWithParentCategory()
    {
        var categoryParent = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Parent Category"
        });

        var category = await SendAsync(new CreateCategoryCommand()
        {
            Name = "New Category With Parent",
            ParentCategoryId = categoryParent.Id
        });

        category.Should().NotBeNull();
        category.Name.Should().Be(category.Name);
        category.ParentCategory.Should().NotBeNull();
        category.ParentCategory!.Id.Should().Be(categoryParent.Id);
        category.Image.Should().BeNull();
    }
}
