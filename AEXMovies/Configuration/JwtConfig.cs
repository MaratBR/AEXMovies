using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AEXMovies.Configuration;

public class JwtConfig
{
    
    public string Issuer { get; set; } = "https://api.movies.aex";
    public TimeSpan TokenLifetime { get; set; } = TimeSpan.FromHours(1);
    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(30);
    public string Audience { get; set; } = "https://movies.aex";
    [Required] public string SecretKey { get; set; } = "wjkertqeiuhgiahefioawhrpfwh4gjieozsgnpeoiiowermcweronsumuhqweeeeeeeeeeeeeeeeeeeeek.qhwmfuyguye";

    public SecurityKey GetSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
    }
}