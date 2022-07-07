namespace AEXMovies.Services.Dtos;

public class GenreDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
}