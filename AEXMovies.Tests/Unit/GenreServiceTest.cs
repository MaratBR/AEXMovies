using AEXMovies.Repositories;
using AEXMovies.Repositories.GenreRepository;
using AEXMovies.Services.GenreService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AEXMovies.Tests.Unit;

public class GenreServiceTest : DatabaseTest
{
    public GenreServiceTest()
    {
        RegisterServices(collection =>
        {
            collection.AddScoped<IGenreRepository, GenreRepository>();
            collection.AddScoped<IGenreService, GenreService>();
        });
    }

    [Fact]
    public async Task TestEnsureGenres()
    {
        var service = BuildOrGetServiceProvider().GetRequiredService<IGenreService>();
        await service.EnsureAllExist(new string[] { "Action", "Adventure", "Science fiction" });
        var context = BuildOrGetServiceProvider().GetRequiredService<EfDbContext>();
        Assert.Equal(3, await context.Genres.CountAsync());
    }
}