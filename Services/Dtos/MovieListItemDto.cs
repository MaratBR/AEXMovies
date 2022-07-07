using AEXMovies.Models;

namespace AEXMovies.Services.Dtos;

public class MovieListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MpaRating Mpa { get; set; }
    public List<GenreDto> Genres { get; set; } = new();
}