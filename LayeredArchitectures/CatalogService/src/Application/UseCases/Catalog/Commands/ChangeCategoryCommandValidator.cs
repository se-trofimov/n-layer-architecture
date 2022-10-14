using FluentValidation;

namespace CatalogService.Application.UseCases.Catalog.Commands
{
    public class ChangeCategoryCommandValidator: AbstractValidator<ChangeCategoryCommand>
    {
        public ChangeCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}
