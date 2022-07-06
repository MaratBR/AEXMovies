namespace AEXMovies.Services.MovieService;

public class SearchOptions
{
    public bool SearchName { get; set; } = true;
    public bool SearchActors { get; set; } = true;
    public bool SearchGenres { get; set; } = true;

}