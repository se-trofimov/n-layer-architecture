using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.UseCases.Catalog.Queries;
using CatalogService.Application.UseCases.Items.Queries;
using FluentAssertions;
using NUnit.Framework;

namespace CatalogService.Application.IntegrationTests;
using static Testing;

public class PaginationTests : BaseTestFixture
{
    [Test]
    public async Task GetCatalogsQuery_WrongPageNumber_ThrowException()
    {
        GetCatalogsQuery query = new(-1, 5);
        await FluentActions.Invoking(() =>
            SendAsync(query)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task GetCatalogsQuery_WrongPageSize_ThrowException()
    {
        GetCatalogsQuery query = new(1, -5);
        await FluentActions.Invoking(() =>
            SendAsync(query)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task GetItemsQuery_WrongPageNumber_ThrowException()
    {
        GetItemsQuery query = new(1, -1, 5);
        await FluentActions.Invoking(() =>
            SendAsync(query)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task GetItemsQuery_WrongPageSize_ThrowException()
    {
        GetItemsQuery query = new(1, 1, -5);
        await FluentActions.Invoking(() =>
            SendAsync(query)).Should().ThrowAsync<ValidationException>();
    }
}
