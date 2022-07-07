using System.Text.RegularExpressions;
using AEXMovies.Models;
using AEXMovies.Repositories.GenreRepository;
using AEXMovies.Services.Dtos;

namespace AEXMovies.Services.GenreService;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    
    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }
    
    public async Task<Genre> GetOrCreate(string name)
    {
        var genre = await _genreRepository.FindOne(g => g.NormalizedName == NormalizeName(name));
        if (genre == null)
        {
            genre = new Genre
            {
                NormalizedName = NormalizeName(name),
                DisplayName = name,
            };
            await _genreRepository.Save(genre);
        }

        return genre;
    }

    public async Task<List<Genre>> EnsureAllExist(IEnumerable<string> names)
    {
        var genreNames = new Dictionary<string, string>(
            names.Select(name => new KeyValuePair<string,string>(NormalizeName(name), name))
            );
        var existing = await _genreRepository.FindMany(g => genreNames.Keys.ToList().Contains(g.NormalizedName));
        var newGenreNames = genreNames.Keys.Except(existing.Select(g => g.NormalizedName));
        var newGenres = newGenreNames.Select(normName => new Genre
        {
            NormalizedName = normName,
            DisplayName = genreNames[normName]
        }).ToList();
        
        await _genreRepository.Save(newGenres);
        existing.AddRange(newGenres);
        return existing;
    }

    public Task<List<GenreDto>> Search(string? query = null)
    {
        if (query == null)
            return _genreRepository.FindAndOrderMany<GenreDto, string>(g => g.NormalizedName, take: 50);
        return _genreRepository.FindAndOrderMany<GenreDto, string>(
            g => g.NormalizedName, 
            g => g.NormalizedName.Contains(query), 
            take: 50);
    }
    
    private static readonly Regex WhiteSpaceRegex = new(@"\s+");

    private static string NormalizeName(string name)
    {
        return WhiteSpaceRegex.Replace(name.ToUpper().Trim(), " ");
    }
}