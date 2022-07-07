using System.Security.Claims;
using AEXMovies.Models;
using AEXMovies.Services.Dtos;

namespace AEXMovies.Services.AuthService;

public interface IAuthService
{
    Task<User?> FindUserByCredentials(UserCredentialsDto credentials);
    Task<ClaimsPrincipal> CreatePrincipal(User user, string authenticationSchema);
    Task<User> RegisterNewUser(NewUserDto dto);
}