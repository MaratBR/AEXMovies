namespace AEXMovies.Services.Dtos;

public class ActorDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Name { get; set; } = string.Empty;
}