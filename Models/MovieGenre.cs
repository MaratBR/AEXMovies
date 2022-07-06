namespace AEXMovies.Models;

public class MovieGenre
{
    public int GenreId { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    public Genre Genre { get; set; } = null!;

}