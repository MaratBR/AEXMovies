using AEXMovies.Models;
using AEXMovies.Services.Dtos;

namespace AEXMovies.Services.MovieService;

public interface IMovieService
{
    Task<MovieDetailsDto?> GetMovie(int id);
    
    /// <summary>
    /// Finds movie that matches the name provided. If there's more than one movie, will
    /// return first movie that matches. Exact order is undefined and depends on default database
    /// ordering.
    /// </summary>
    /// <param name="fullName">Full movie name</param>
    /// <returns>Task that represents asynchronous operation.</returns>
    Task<MovieDetailsDto?> FindMovieByName(string fullName);
    Task<MovieDto> UpdateMovie(int id, UpdateMovieDto dto);
    Task DeleteById(int id);
    Task<List<MovieListItemDto>> SearchMovies(string? query, SearchOptions options);
    Task<List<MovieListItemDto>> SearchMovies(AdvancedSearchOptions options);
    Task<MovieDto> CreateMovie(CreateNewMovieDto newMovieDto);
}