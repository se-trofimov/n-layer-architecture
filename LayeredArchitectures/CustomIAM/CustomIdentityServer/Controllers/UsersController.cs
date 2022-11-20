using CustomIdentityServer.UIModels;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentityServer.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserIdentityService _userIdentityService;
    private readonly IValidator<CreationUser> _creationUserValidator;

    public UsersController(UserIdentityService userIdentityService,
        IValidator<CreationUser> creationUserValidator)
    {
        _userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
        _creationUserValidator = creationUserValidator ?? throw new ArgumentNullException(nameof(creationUserValidator));
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(CreationUser user)
    {
        var validationResult = await _creationUserValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var newUser = await _userIdentityService.CreateUserAsync(user.Name!, user.Email!, user.Password!);
            return Ok(newUser.Adapt<User>());
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
}
