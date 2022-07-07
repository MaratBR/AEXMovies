using AEXMovies.Models;
using AEXMovies.Repositories;
using AEXMovies.Repositories.ActorRepository;
using AEXMovies.Repositories.GenreRepository;
using AEXMovies.Repositories.MovieRepository;
using AEXMovies.Services.Dtos;
using AEXMovies.Services.MovieService;
using AEXMovies.Services.MovieService.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AEXMovies.Tests.Unit;

public class MovieServiceTest : DatabaseTest
{
    private IMovieService MovieService() => BuildOrGetServiceProvider().GetRequiredService<IMovieService>();

    public MovieServiceTest()
    {
        RegisterServices(collection =>
        {
            collection.AddScoped<IMovieRepository, MovieRepository>();
            collection.AddScoped<IActorRepository, ActorRepository>();
            collection.AddScoped<IGenreRepository, GenreRepository>();
            collection.AddScoped<IMovieService, MovieService>();
        });
    }

    [Fact]
    public async Task CanCreateMovie()
    {
        await MovieService().CreateMovie(new CreateNewMovieDto
        {
            Name = "My movie"
        });
        Assert.Equal(1, await BuildOrGetServiceProvider().GetRequiredService<EfDbContext>().Movies.CountAsync());
    }
    
    [Fact]
    public async Task CanCreateMovieWithActorsAndGenres()
    {
        var ctx = BuildOrGetServiceProvider().GetRequiredService<EfDbContext>();
        
        ctx.AddRange(
            new Genre
            {
                NormalizedName = "TEST",
                DisplayName = "Test"
            },
            new Actor()
            {
                Name = "Robert De Niro"
            }
            );
        await ctx.SaveChangesAsync();
        
        await MovieService().CreateMovie(new CreateNewMovieDto
        {
            Name = "My movie",
            GenreIds = new List<int> { 1 },
            ActorIds = new List<int> { 1 }
        });
        Assert.Equal(1, await ctx.Movies.CountAsync());
    }
    
    [Theory]
    [InlineData("genre")]
    [InlineData("actor")]
    [InlineData("both")]
    public async Task CannotCreateMovieWithInvalidActorOrGenre(string mode)
    {
        var ctx = BuildOrGetServiceProvider().GetRequiredService<EfDbContext>();

        await Assert.ThrowsAnyAsync<MovieException>(async () =>
        {
            await MovieService().CreateMovie(new CreateNewMovieDto
            {
                Name = "My movie",
                GenreIds = mode == "both" || mode == "genre" ? new List<int> { 1 } : new(),
                ActorIds = mode == "both" || mode == "actor" ? new List<int> { 1 } : new(),
            });
        });
        Assert.Equal(0, await ctx.Movies.CountAsync());
    }

    [Theory]
    [InlineData("Avengers: Infinity War", new [] { "Some random dude", "Homeless person we found on the street" })]
    public async Task TestSimpleSearch(string movieName, string[] actors)
    {
        var ctx = BuildOrGetServiceProvider().GetRequiredService<EfDbContext>();
        var actorInst = actors.Select(name => new Actor() { Name = name }).ToList();
        ctx.Actors.AddRange(actorInst);
        ctx.Genres.Add(new Genre() { NormalizedName = "ACTION", DisplayName = "Action" });
        var movie = new Movie() { Name = movieName };
        ctx.Movies.Add(movie);
        await ctx.SaveChangesAsync();
        movie.Actors = actorInst.Select(a => new MovieActor { ActorId = a.Id, MovieId = movie.Id }).ToList();
        movie.Genres = new List<MovieGenre> { new MovieGenre { MovieId = movie.Id, GenreId = 1 } };
        await ctx.SaveChangesAsync();

        var movieService = BuildOrGetServiceProvider().GetRequiredService<IMovieService>();
        Assert.Single((await movieService.SearchMovies("Avengers", new SearchOptions())));
        Assert.Single((await movieService.SearchMovies("Homeless", new SearchOptions())));
        Assert.Single((await movieService.SearchMovies("dude", new SearchOptions())));
        Assert.Single((await movieService.SearchMovies("Action", new SearchOptions())));
    }
    
}