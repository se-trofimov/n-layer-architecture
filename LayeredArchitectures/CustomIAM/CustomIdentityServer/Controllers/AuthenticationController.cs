using Microsoft.AspNetCore.Mvc;

namespace CustomIdentityServer.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserIdentityService _userIdentityService;
    

    public AuthenticationController(UserIdentityService userIdentityService)
    {
        _userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
    }

    [HttpGet("access_token")]
    public async Task<IActionResult> AccessToken(string code, string clientSecret)
    {
        try
        {
            var token = await _userIdentityService.GetAccessTokenAsync(code, clientSecret);
            return Ok(token);
        }
        catch (Exception e)
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

        var code = await _userIdentityService.LoginAsync(user, password);

        return Ok(code);
    }
}
