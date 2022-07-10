using AEXMovies.Models;
using AEXMovies.Services.Dtos;

namespace AEXMovies.Services.AuthService;

public interface IAuthService
{
    /// <summary>
    /// Fins user by given credentials (login and password). Is no user found returns null.
    /// </summary>
    /// <param name="credentials">Credentials object</param>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<User?> FindUserByCredentials(UserCredentialsDto credentials);
    
    /// <summary>
    /// Registers new user based on the user info provided.
    /// </summary>
    /// <param name="dto">DTO object with new user's data.</param>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<User> RegisterNewUser(NewUserDto dto);
    
    /// <summary>
    /// Generates new user access token as a string.
    /// </summary>
    /// <param name="user">User for whom the token should be generated.</param>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<string> GenerateUserToken(User user);
    
    /// <summary>
    /// Creates new refresh token and returns the string representation of it.
    /// </summary>
    /// <param name="user">User for whom the token should be generated.</param>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<RefreshToken> CreateNewRefreshToken(User user);
    
    /// <summary>
    /// Rotates refresh token by invalidating old one and creating a new token
    /// based on the old one.
    /// </summary>
    /// <param name="refreshToken">Old refresh token.</param>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<RefreshToken> RotateRefreshToken(RefreshToken refreshToken);
    
    /// <summary>
    /// Finds the user to whom the token belong.
    /// </summary>
    /// <param name="token">Refresh token model.</param>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<User> FindUserByRefreshToken(RefreshToken token);
    
    /// <summary>
    /// Search for refresh token model with given string representation.
    /// </summary>
    /// <param name="token">String representation of the token.</param>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<RefreshToken?> FindRefreshToken(string token);
}