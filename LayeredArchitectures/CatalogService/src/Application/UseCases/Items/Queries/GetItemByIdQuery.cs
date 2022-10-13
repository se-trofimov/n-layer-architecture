using AutoMapper;
using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Items.Queries;
public class GetItemByIdQuery : IRequest<ItemDto>
{
    public GetItemByIdQuery(int id, int categoryId)
    {
        Id = id;
        CategoryId = categoryId;
    }
    public int Id { get; }
    public int CategoryId { get; }
}

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDto>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetItemByIdQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<ItemDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _applicationDbContext.Items
            .Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id 
                                      && x.CategoryId == request.CategoryId, cancellationToken: cancellationToken);
        if (item is null)
            throw new NotFoundException($"Item with id {request.Id} not found");
        return _mapper.Map<ItemDto>(item);
    }
}
