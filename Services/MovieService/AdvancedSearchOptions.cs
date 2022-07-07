using System.ComponentModel.DataAnnotations;

namespace AEXMovies.Services.MovieService;

public class AdvancedSearchOptions
{
    public List<int> GenreIds { get; set; } = new();
    public List<int> ActorIds { get; set; } = new();

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string? NameQuery { get; set; } = string.Empty;
}