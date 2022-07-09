using AEXMovies.Models;
using AEXMovies.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AEXMovies.Tests.Unit;

public class EfRepositoryTest : DatabaseTest
{
    public EfRepositoryTest()
    {
        RegisterServices(collection => { collection.AddScoped<EfRecordRepository<Movie>>(); });
    }

    [Fact]
    public async Task Get()
    {
        var repo = BuildOrGetServiceProvider().GetRequiredService<EfRecordRepository<Movie>>();

        await repo.Save(new Movie { Name = "Movie" });
        var movie = await repo.GetById(1);
        Assert.NotNull(movie);
    }

    [Fact]
    public async Task FindMany()
    {
        var repo = BuildOrGetServiceProvider().GetRequiredService<EfRecordRepository<Movie>>();

        await repo.Save(new Movie { Name = "Movie" });

        var movies = await repo.FindMany();
        Assert.Single(movies);
    }

    [Fact]
    public async Task FindOne()
    {
        var repo = BuildOrGetServiceProvider().GetRequiredService<EfRecordRepository<Movie>>();

        await repo.Save(new Movie { Name = "Movie" });
        var movie = await repo.FindOne(m => m.Id == 1);
        Assert.NotNull(movie);
    }
}