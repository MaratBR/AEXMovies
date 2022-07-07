using AEXMovies.Services.AuthService;
using AEXMovies.Services.AuthService.Exceptions;
using AEXMovies.Services.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AEXMovies.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [Authorize]
    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        return Ok(new
        {
            Claims = new Dictionary<string, string>(
                HttpContext.User.Claims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value)))
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _authService.FindUserByCredentials(dto);
        if (user == null)
            return Unauthorized("Invalid user credentials");
        var principal = await _authService.CreatePrincipal(user, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return Ok(new { Message = "Logged in successfully" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(NewUserDto dto)
    {
        try
        {
            var user = await _authService.RegisterNewUser(dto);
            return Ok(new { Message = $"Welcome aboard, {user.UserName}!" });
        }
        catch (UserAlreadyExists e)
        {
            return Conflict(e.Message);
        }
    }
}