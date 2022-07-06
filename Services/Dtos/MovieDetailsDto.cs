using AEXMovies.Models;

namespace AEXMovies.Services.Dtos;

public class MovieDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MpaRating Mpa { get; set; }
    public List<ActorDto> Actors { get; set; } = new();
    public List<GenreDto> Genres { get; set; } = new();
}