using System.ComponentModel.DataAnnotations;

namespace AEXMovies.Services.Dtos;

public class CreateActorDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public string Name { get; set; } = string.Empty;
}