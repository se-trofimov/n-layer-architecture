using Bogus;
using MediatR;

namespace CatalogService.Application.UseCases.Items.Queries;

public class GetItemPropertiesQuery : IRequest<Dictionary<string, string>>
{
    public int Id { get; init; }
}

public class GetItemPropertiesQueryHandler : IRequestHandler<GetItemPropertiesQuery, Dictionary<string, string>>
{
    public Task<Dictionary<string, string>> Handle(GetItemPropertiesQuery request, CancellationToken cancellationToken)
    {
        Faker faker = new Faker();
        Dictionary<string, string> properties = new()
        {
            {"Material", faker.Commerce.ProductMaterial()},
            {"Color", faker.Commerce.Color()},
            {"EAN13", faker.Commerce.Ean13()},
            {"Producer", faker.Address.Country()}
        };
        return Task.FromResult(properties);
    }
}