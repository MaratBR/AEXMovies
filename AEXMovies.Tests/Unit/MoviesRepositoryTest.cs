using AEXMovies.Models;
using AEXMovies.Repositories;
using AEXMovies.Repositories.MovieRepository;
using AEXMovies.Services.Dtos;
using AEXMovies.Services.MovieService;
using Microsoft.Extensions.DependencyInjection;

namespace AEXMovies.Tests.Unit;

public class MoviesRepositoryTest : DatabaseTest
{
    public MoviesRepositoryTest()
    {
        RegisterServices(collection => { collection.AddScoped<IMovieRepository, MovieRepository>(); });
    }

    [Fact]
    public async Task TestUpdateActors()
    {
        var context = BuildOrGetServiceProvider().GetRequiredService<EfDbContext>();

        var actors = Enumerable.Range(1, 20).Select(i => new Actor { Name = $"Actor #{i}" }).ToList();
        context.Actors.AddRange(actors);
        await context.SaveChangesAsync();

        var movieRepository = BuildOrGetServiceProvider().GetRequiredService<IMovieRepository>();
        await movieRepository.Save(new Movie { Name = "Movie" });
        var movie = await movieRepository.GetById(1);
        Assert.NotNull(movie);
        await movieRepository.UpdateActors(movie, actors);
    }
}