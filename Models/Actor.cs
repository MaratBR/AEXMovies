using AEXMovies.Repositories;

namespace AEXMovies.Models;

public class Actor : RecordModel
{
    public string Name { get; set; } = string.Empty;

    public ICollection<MovieActor> Movies { get; set; } = new List<MovieActor>();
}