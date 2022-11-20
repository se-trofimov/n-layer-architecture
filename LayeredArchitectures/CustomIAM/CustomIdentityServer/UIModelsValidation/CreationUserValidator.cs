using CustomIdentityServer.UIModels;
using FluentValidation;

namespace CustomIdentityServer.UIModelsValidation;

public class CreationUserValidator: AbstractValidator<CreationUser>
{
    public CreationUserValidator()
    {
        RuleFor(x=>x.Name).NotEmpty();
        RuleFor(x=>x.Email).NotEmpty();
        RuleFor(x=>x.Password).NotEmpty();
    }
}
