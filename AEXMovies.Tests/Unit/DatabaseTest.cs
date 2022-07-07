using AEXMovies.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AEXMovies.Tests.Unit;

public class DatabaseTest : BaseTest, IDisposable
{
    private readonly Guid dbUUID = Guid.NewGuid();
    
    public DatabaseTest()
    {
        RegisterServices(collection =>
        {
            collection.AddDbContext<EfDbContext>(options =>
            {
                options.UseSqlServer($"Server=localhost;Database=AEXMovies.Testing.{dbUUID:N};User=sa;Password=Password$");
            });
        });
        
        OnServiceProvider(provider =>
        {
            provider.GetRequiredService<EfDbContext>().Database.EnsureCreated();
        });
    }

    public void Dispose()
    {
        BuildOrGetServiceProvider().GetRequiredService<EfDbContext>().Database.EnsureDeleted();
    }
}