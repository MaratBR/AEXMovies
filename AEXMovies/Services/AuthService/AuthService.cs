using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AEXMovies.Configuration;
using AEXMovies.Models;
using AEXMovies.Repositories.RefreshTokenRepository;
using AEXMovies.Services.AuthService.Exceptions;
using AEXMovies.Services.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AEXMovies.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtConfig _jwtConfig;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(UserManager<User> userManager, JwtConfig jwtConfig, IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtConfig = jwtConfig;
    }

    public async Task<User?> FindUserByCredentials(UserCredentialsDto credentials)
    {
        var user = await _userManager.FindByNameAsync(credentials.Login);
        if (user == null)
            return null;

        if (!await _userManager.CheckPasswordAsync(user, credentials.Password))
            return null;

        return user;
    }

    public async Task<User> RegisterNewUser(NewUserDto dto)
    {
        var user = new User
        {
            UserName = dto.UserName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded)
            return user;
        throw new UserIdentityErrorsException(result.Errors.ToList());
    }

    public async Task<string> GenerateUserToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            _jwtConfig.Issuer,
            expires: DateTime.UtcNow.Add(_jwtConfig.TokenLifetime),
            claims: claims,
            audience: _jwtConfig.Audience,
            signingCredentials: new SigningCredentials(
                _jwtConfig.GetSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    
    public Task<RefreshToken> CreateNewRefreshToken(User user)
    {
        return CreateNewRefreshToken(user.Id);
    }

    public async Task<RefreshToken> CreateNewRefreshToken(string userId)
    {
        var token = new RefreshToken
        {
            CreatedById = userId,
            Id = GenerateRefreshTokenId()
        };
        await _refreshTokenRepository.Insert(token);
        return token;
    }

    public async Task<RefreshToken> RotateRefreshToken(RefreshToken refreshToken)
    {
        await _refreshTokenRepository.Delete(refreshToken);
        return await CreateNewRefreshToken(refreshToken.CreatedById);
    }

    public Task<User> FindUserByRefreshToken(RefreshToken token)
    {
        return _userManager.FindByIdAsync(token.CreatedById);
    }

    public Task<RefreshToken?> FindRefreshToken(string token)
    {
        return _refreshTokenRepository.Get(token);
    }

    private string GenerateRefreshTokenId()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(20));
    }
}