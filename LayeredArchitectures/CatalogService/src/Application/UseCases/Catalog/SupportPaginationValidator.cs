using CatalogService.Application.Common;
using FluentValidation;

namespace CatalogService.Application.UseCases.Catalog;
public class SupportPaginationValidator<T> : AbstractValidator<T> where T: SupportPagination
{
    public SupportPaginationValidator()
    {
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
        RuleFor(x => x.PageNumber).GreaterThan(0);
    }
}
