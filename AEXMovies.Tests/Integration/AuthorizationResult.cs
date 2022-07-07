using AEXMovies.Models;

namespace AEXMovies.Tests.Integration;

public class AuthorizationResult
{
    public AuthorizationResult(User user, string password, string token, string refreshToken)
    {
        User = user;
        Password = password;
        Token = token;
        RefreshToken = refreshToken;
    }

    public User User { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}