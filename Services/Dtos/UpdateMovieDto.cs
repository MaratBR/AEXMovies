using System.ComponentModel.DataAnnotations;
using AEXMovies.Models;

namespace AEXMovies.Services.Dtos;

public class UpdateMovieDto
{
    [MinLength(1)]
    [MaxLength(255)]
    public string? Name { get; set; } = string.Empty;

    public MpaRating? Mpa { get; set; } = MpaRating.None;

    public List<int>? ActorIds { get; set; } = new();

    public List<int>? GenreIds { get; set; } = new();
}