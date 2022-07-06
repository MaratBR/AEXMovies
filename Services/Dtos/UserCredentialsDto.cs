using System.ComponentModel.DataAnnotations;

namespace AEXMovies.Services.Dtos;

public class UserCredentialsDto
{
    [MinLength(1)] [Required]
    public string Login { get; set; } = string.Empty;
    [MinLength(1)] [Required]
    public string Password { get; set; } = string.Empty;

}