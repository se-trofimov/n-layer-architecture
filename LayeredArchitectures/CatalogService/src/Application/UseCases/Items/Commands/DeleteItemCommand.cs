using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Items.Commands;
public class DeleteItemCommand: IRequest
{
    public DeleteItemCommand(int id, int categoryId)
    {
        Id = id;
        CategoryId = categoryId;
    }

    public int CategoryId { get; }
    public int Id { get; }
}

public class DeleteItemCommandHandler: IRequestHandler<DeleteItemCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public DeleteItemCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
    }
    public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _applicationDbContext.Items
            .FirstOrDefaultAsync(x => x.Id == request.Id 
                                      && x.CategoryId == request.CategoryId, cancellationToken: cancellationToken);
        if (item is null)
            throw new NotFoundException($"Item with id {request.Id} and categoryId {request.CategoryId} not found");
        _applicationDbContext.Items.Remove(item);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}