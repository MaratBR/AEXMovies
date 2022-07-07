using AEXMovies.Services.AuthService;
using AEXMovies.Services.AuthService.Exceptions;
using AEXMovies.Services.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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