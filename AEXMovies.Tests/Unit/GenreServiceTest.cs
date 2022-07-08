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
    public async Task EnsureGenres()
    {
        var service = BuildOrGetServiceProvider().GetRequiredService<IGenreService>();
        await service.EnsureAllExist(new string[] { "Action", "Adventure", "Science fiction" });
        var context = BuildOrGetServiceProvider().GetRequiredService<EfDbContext>();
        Assert.Equal(3, await context.Genres.CountAsync());
    }
    
    
    [Fact]
    public async Task Search()
    {
        var service = BuildOrGetServiceProvider().GetRequiredService<IGenreService>();
        await service.EnsureAllExist(new string[] { "Action", "Adventure", "Science fiction" });
        var result = await service.Search("A");
        Assert.All(result, g => Assert.StartsWith("A", g.NormalizedName));
        Assert.Equal(2, result.Count);
    }
}