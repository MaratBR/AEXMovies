namespace AEXMovies.Services.MovieService.Exceptions;

[Serializable]
public class InvalidGenreIds : MovieException
{
    public InvalidGenreIds(IEnumerable<int> genreIds) : base($"Genres with following IDs could not be found: {string.Join(", ", genreIds)}")
    {
        
    }
}