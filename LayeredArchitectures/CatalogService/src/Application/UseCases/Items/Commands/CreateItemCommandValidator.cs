using FluentValidation;

namespace CatalogService.Application.UseCases.Items.Commands
{
    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
