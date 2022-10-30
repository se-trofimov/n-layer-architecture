using AutoMapper;
using AutoMapper.QueryableExtensions;
using CatalogService.Application.Common;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Catalog.Queries;
public class GetCatalogsQuery: SupportPagination, IRequest<PagedList<CategoryDto>>
{
    public GetCatalogsQuery(int pageNumber, int pageSize)
        :base(pageNumber, pageSize)
    {
        
    }
}

public class GetCatalogQueryHandler : IRequestHandler<GetCatalogsQuery, PagedList<CategoryDto>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetCatalogQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedList<CategoryDto>> Handle(GetCatalogsQuery request, CancellationToken cancellationToken)
    {
        var res = await _applicationDbContext.Categories
            .Include(x=>x.ParentCategory)
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToPagedList(request.PageNumber, request.PageSize);
        return res;
    }
}