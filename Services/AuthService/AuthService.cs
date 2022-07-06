using System.Security.Claims;
using AEXMovies.Models;
using AEXMovies.Services.Dtos;
using Microsoft.AspNetCore.Identity;

namespace AEXMovies.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;

    public async Task<User?> FindUserByCredentials(UserCredentialsDto credentials)
    {
        var user = await _userManager.FindByNameAsync(credentials.Login);
        if (user == null)
            return null;

        if (!await _userManager.CheckPasswordAsync(user, credentials.Password))
            return null;

        return user;
    }

    public async Task<ClaimsPrincipal> CreatePrincipal(User user, string authenticationSchema)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, string.Join(",", roles))
        };
        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationSchema));
    }

    public async Task<User> RegisterNewUser(NewUserDto dto)
    {
        var user = new User
        {
            UserName = dto.UserName,
        };

        await _userManager.CreateAsync(user, dto.Password);
        return user;
    }
}