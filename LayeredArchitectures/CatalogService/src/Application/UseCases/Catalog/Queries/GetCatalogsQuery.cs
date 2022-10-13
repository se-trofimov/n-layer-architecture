using AutoMapper;
using AutoMapper.QueryableExtensions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Catalog.Queries;
public class GetCatalogsQuery: IRequest<CategoryDto[]>
{

}

public class GetCatalogQueryHandler : IRequestHandler<GetCatalogsQuery, CategoryDto[]>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetCatalogQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CategoryDto[]> Handle(GetCatalogsQuery request, CancellationToken cancellationToken)
    {
        return await _applicationDbContext.Categories
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken: cancellationToken);
    }
}