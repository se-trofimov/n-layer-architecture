using AutoMapper;
using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Catalog.Queries;
public class GetCategoryByIdQuery: IRequest<CategoryDto>
{
    public GetCategoryByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; }
}

public class GetCatalogByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetCatalogByIdQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        await _applicationDbContext.Categories.LoadAsync(cancellationToken: cancellationToken);
        var category = await _applicationDbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (category is null)
            throw new NotFoundException($"Category with id {request.Id} not found");
        return _mapper.Map<CategoryDto>(category);
    }
}
