namespace AEXMovies.Services.Dtos;

public class ActorDetailsDto : ActorDto
{
    public List<MovieDto> Movies { get; set; } = new();
}