using AEXMovies.Models;
using AEXMovies.Services.Dtos;

namespace AEXMovies.Services.GenreService;

public interface IGenreService
{
    Task<Genre> GetOrCreate(string name);
    Task<List<Genre>> EnsureAllExist(IEnumerable<string> names);
    Task<List<GenreDto>> Search(string? query = null);
}