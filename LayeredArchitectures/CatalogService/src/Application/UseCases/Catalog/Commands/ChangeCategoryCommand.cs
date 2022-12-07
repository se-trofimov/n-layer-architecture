using System.Text.Json.Serialization;
using AutoMapper;
using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Catalog.Commands;
public class ChangeCategoryCommand : IRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Image { get; set; }
    public int? ParentCategoryId { get; set; }
}
 
public class ChangeCategoryCommandHandler : IRequestHandler<ChangeCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public ChangeCategoryCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<Unit> Handle(ChangeCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (category is null)
            throw new NotFoundException($"Category with id {request.Id} not found");
        _mapper.Map(request, category);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
