using System.Linq.Expressions;
using AEXMovies.Models;
using AEXMovies.Services.Dtos;
using AEXMovies.Services.MovieService;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AEXMovies.Repositories.MovieRepository;

public class MovieRepository : EfRecordRepository<Movie>, IMovieRepository
{
    public MovieRepository(EfDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public Task UpdateGenres(Movie movie, List<Genre> genres)
    {
        movie.Genres = genres.Select(g => new MovieGenre() { MovieId = movie.Id, GenreId = g.Id }).ToList();
        return Save(movie);
    }

    public Task UpdateActors(Movie movie, List<Actor> actors)
    {
        movie.Actors = actors.Select(a => new MovieActor() { MovieId = movie.Id, ActorId = a.Id }).ToList();
        return Save(movie);
    }
}