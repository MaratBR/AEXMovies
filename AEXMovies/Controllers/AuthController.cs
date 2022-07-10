using AEXMovies.Services.AuthService;
using AEXMovies.Services.AuthService.Exceptions;
using AEXMovies.Services.Dtos;
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


    [HttpGet("whoami")]
    [Authorize]
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

        var refreshToken = await _authService.CreateNewRefreshToken(user);
        var token = await _authService.GenerateUserToken(user);

        return Ok(new
        {
            Token = token,
            RefreshToken = refreshToken.Id
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto dto)
    {
        var oldRefreshToken = await _authService.FindRefreshToken(dto.RefreshToken);
        if (oldRefreshToken == null)
            return Unauthorized("Invalid refresh token");

        var refreshToken = await _authService.RotateRefreshToken(oldRefreshToken);
        var user = await _authService.FindUserByRefreshToken(refreshToken);
        var token = await _authService.GenerateUserToken(user);

        return Ok(new
        {
            Token = token,
            RefreshToken = refreshToken
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(NewUserDto dto)
    {
        try
        {
            var user = await _authService.RegisterNewUser(dto);
            return Ok(new { Message = $"Welcome aboard, {user.UserName}!" });
        }
        catch (UserIdentityErrorsException e)
        {
            // TODO: more concrete status code?
            return BadRequest(new
            {
                e.Errors, e.Message
            });
        }
    }
}