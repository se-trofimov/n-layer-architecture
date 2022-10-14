using CartingService.UIContracts;
using FluentValidation;

namespace CartingService.Validators
{
    public class ItemsValidator:AbstractValidator<Item>
    {
        public ItemsValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Image).NotEmpty();
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
