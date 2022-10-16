using AutoMapper;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Dtos;
using CatalogService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.UseCases.Catalog.Commands;
public class CreateCategoryCommand: IRequest<CategoryDto>
{
    public string Name { get; set; }
    public string? Image { get; set; }
    public int? ParentCategoryId { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        if (mapper == null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var newCategory = _mapper.Map<Category>(request);
        _applicationDbContext.Categories.Add(newCategory);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        var createdCategory = await _applicationDbContext.Categories
            .AsNoTracking()
            .Include(x=>x.ParentCategory)
            .FirstOrDefaultAsync(x => x.Id == newCategory.Id, cancellationToken: cancellationToken);

        return _mapper.Map<CategoryDto>(createdCategory);
    }
}
