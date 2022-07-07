using AEXMovies.Models;

namespace AEXMovies.Repositories.MovieRepository;

public interface IMovieRepository : IRecordRepository<Movie>
{
    Task UpdateGenres(Movie movie, List<Genre> genres);
    Task UpdateActors(Movie movie, List<Actor> actors);
}