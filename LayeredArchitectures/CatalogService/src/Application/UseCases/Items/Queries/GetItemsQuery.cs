using AutoMapper;
using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Items.Queries;
public class GetItemsQuery: IRequest<ItemDto[]>
{
    public GetItemsQuery(int categoryId)
    {
        CategoryId = categoryId;
    }
    public int CategoryId { get; set; }
}

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, ItemDto[]>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetItemsQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<ItemDto[]> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var category = await _applicationDbContext.Categories
            .AsNoTracking()
            .Include(x=>x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken: cancellationToken);
        if (category is null)
            throw new NotFoundException($"Category with id {request.CategoryId} not found");

        return _mapper.Map<ItemDto[]>(category.Items);
    }
}
