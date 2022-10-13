using System.Text.Json.Serialization;
using AutoMapper;
using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Items.Commands;
public class ChangeItemCommand: IRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public int CategoryId { get; set; }
}

public class ChangeItemCommandHandler : IRequestHandler<ChangeItemCommand, Unit>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public ChangeItemCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Unit> Handle(ChangeItemCommand request, CancellationToken cancellationToken)
    {
        var category = await _applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken: cancellationToken);
        if (category is null)
            throw new NotFoundException($"Category with id {request.CategoryId} not found");
        var item = _applicationDbContext.Items.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (item is null)
            throw new NotFoundException($"Item with id {request.Id} not found");
        _ = _mapper.Map(request, item);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
