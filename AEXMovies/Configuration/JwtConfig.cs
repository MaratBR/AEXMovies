using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AEXMovies.Configuration;

public class JwtConfig
{
    [Required] public string Issuer { get; set; } = string.Empty;

    [Required] public TimeSpan TokenLifetime { get; set; } = TimeSpan.FromHours(1);

    [Required] public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(30);

    [Required] public string Audience { get; set; } = string.Empty;

    [Required] public string SecretKey { get; set; } = string.Empty;

    public SecurityKey GetSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
    }
}