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
    private readonly IJwtUtils _jwtUtils;
    private readonly IValidator<CreationUser> _creationUserValidator;

    public UsersController(UserIdentityService userIdentityService,
        IJwtUtils jwtUtils,
        IValidator<CreationUser> creationUserValidator)
    {
        _userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
        _jwtUtils = jwtUtils ?? throw new ArgumentNullException(nameof(jwtUtils));
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

    [HttpGet("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _userIdentityService.GetUserByEmailAsync(email);
        if (user == null)
            return NotFound($"User with email {email} not found!");
        
        var passwordIsValid = _userIdentityService.PasswordIsValid(user, password);

        if (passwordIsValid)
            return BadRequest("Password is invalid");

        var token = _jwtUtils.GenerateToken(user);
        return Ok(token);
    }
}
