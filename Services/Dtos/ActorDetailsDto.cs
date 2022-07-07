namespace AEXMovies.Services.Dtos;

public class ActorDetailsDto : ActorDto
{
    public List<MovieListItemDto> Movies { get; set; } = new();
}