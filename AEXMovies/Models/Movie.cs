namespace AEXMovies.Models;

public class Movie : RecordModel
{
    public string Name { get; set; } = string.Empty;
    public MpaRating Mpa { get; set; }

    public ICollection<MovieActor> Actors { get; set; } = new List<MovieActor>();
    public ICollection<MovieGenre> Genres { get; set; } = new List<MovieGenre>();
}