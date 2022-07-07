using System.Security.Claims;
using AEXMovies.Models;
using AEXMovies.Services.Dtos;

namespace AEXMovies.Services.AuthService;

public interface IAuthService
{
    Task<User?> FindUserByCredentials(UserCredentialsDto credentials);
    Task<User> RegisterNewUser(NewUserDto dto);
    Task<string> GenerateUserToken(User user);
    Task<RefreshToken> CreateNewRefreshToken(User user);
    Task<RefreshToken> RotateRefreshToken(RefreshToken refreshToken);
    Task<User> FindUserByRefreshToken(RefreshToken token);
    Task<RefreshToken?> FindRefreshToken(string token);
}