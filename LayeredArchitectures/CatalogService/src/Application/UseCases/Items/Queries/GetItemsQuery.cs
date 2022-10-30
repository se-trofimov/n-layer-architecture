using AutoMapper;
using AutoMapper.QueryableExtensions;
using CatalogService.Application.Common;
using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Items.Queries;
public class GetItemsQuery : SupportPagination, IRequest<PagedList<ItemDto>>
{
    public GetItemsQuery(int categoryId, int pageNum, int pageSize)
    : base(pageNum, pageSize)
    {
        CategoryId = categoryId;
    }
    public int CategoryId { get; set; }
}

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, PagedList<ItemDto>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetItemsQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<PagedList<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var categoryExists = await _applicationDbContext.Categories.AsNoTracking()
            .AnyAsync(x => x.Id == request.CategoryId, cancellationToken: cancellationToken);

        if (!categoryExists)
            throw new NotFoundException($"Category with id {request.CategoryId} not found");

        var items = await _applicationDbContext.Items
            .AsNoTracking()
            .Where(x => x.CategoryId == request.CategoryId)
            .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
            .ToPagedList(request.PageNumber, request.PageSize);
        
        return items;
    }
}
