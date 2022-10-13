using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Catalog.Commands;
public class DeleteCategoryCommand : IRequest
{
    public DeleteCategoryCommand(int id) { Id = id; }
    public int Id { get; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public DeleteCategoryCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (category is null)
            throw new NotFoundException($"Category with id {request.Id} not found");
        _applicationDbContext.Categories.Remove(category);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
