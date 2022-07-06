using AEXMovies.Repositories;

namespace AEXMovies.Models;

public class Genre : RecordModel
{
    public string NormalizedName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}