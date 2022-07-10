using AEXMovies.Services.Dtos;

namespace AEXMovies.Services.MovieService;

public interface IMovieService
{
    Task<MovieDetailsDto?> GetMovie(int id);

    /// <summary>
    ///     Finds movie that matches the name provided. If there's more than one movie, will
    ///     return first movie that matches. Exact order is undefined and depends on default database
    ///     ordering.
    /// </summary>
    /// <param name="fullName">Full movie name</param>
    /// <returns>Task that represents asynchronous operation.</returns>
    Task<MovieDetailsDto?> FindMovieByName(string fullName);

    Task<MovieDto> UpdateMovie(int id, UpdateMovieDto dto);
    Task DeleteById(int id);
    
    /// <summary>
    /// Runs a search to find movies related to the provided query. If query is null,
    /// returns a selection of movies to display to the user.
    /// </summary>
    /// <param name="query">Query string or null.</param>
    /// <param name="options">Search options.</param>
    /// <returns>Task whose value is the list of movies in the search result.</returns>
    Task<List<MovieListItemDto>> SearchMovies(string? query, SearchOptions options);
    
    /// <summary>
    /// Runs an advanced search to find movies the match given options.
    /// </summary>
    /// <param name="options">Search options</param>
    /// <returns>Task whose value is the list of movies in the search result.</returns>
    Task<List<MovieListItemDto>> SearchMovies(AdvancedSearchOptions options);
    Task<MovieDto> CreateMovie(CreateNewMovieDto newMovieDto);
}