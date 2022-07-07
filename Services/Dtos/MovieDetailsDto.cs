using AEXMovies.Models;

namespace AEXMovies.Services.Dtos;

public class MovieDetailsDto : MovieDto
{
    public List<ActorDto> Actors { get; set; } = new();
    public List<GenreDto> Genres { get; set; } = new();
}