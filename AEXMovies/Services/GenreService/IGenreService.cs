using AEXMovies.Models;
using AEXMovies.Services.Dtos;

namespace AEXMovies.Services.GenreService;

public interface IGenreService
{
    Task<Genre> GetOrCreate(string name);
    
    /// <summary>
    /// Ensures that all given genres exist.
    /// </summary>
    /// <param name="names">Genres</param>
    /// <returns>List of genre models.</returns>
    Task<List<Genre>> EnsureAllExist(IEnumerable<string> names);
    
    /// <summary>
    /// Search for genres whose name include the query. If query is null,
    /// returns a selection of genres to display to the user.
    /// </summary>
    /// <param name="query">Genre name query.</param>
    /// <returns></returns>
    Task<List<GenreDto>> Search(string? query = null);
}