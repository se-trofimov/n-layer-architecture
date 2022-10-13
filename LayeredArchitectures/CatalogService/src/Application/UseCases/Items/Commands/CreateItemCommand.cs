using System.Text.Json.Serialization;
using AutoMapper;
using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Dtos;
using CatalogService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Items.Commands;
public class CreateItemCommand: IRequest<ItemDto>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    [JsonIgnore]
    public int CategoryId { get; set; }
}

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemDto>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateItemCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var category = await _applicationDbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken: cancellationToken);
        if (category is null)
            throw new NotFoundException($"Category with id {request.CategoryId} not found");
        var newItem = _mapper.Map<Item>(request);
        _applicationDbContext.Items.Add(newItem);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ItemDto>(newItem);
    }
}
