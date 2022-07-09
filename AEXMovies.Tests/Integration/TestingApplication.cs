using AEXMovies.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AEXMovies.Tests.Integration;

public class TestingApplication : WebApplicationFactory<Program>
{
    private readonly Guid _appId = Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
            services.AddMvcCore().AddApplicationPart(typeof(Program).Assembly);
            services.AddScoped(sp => new DbContextOptionsBuilder<EfDbContext>()
                .UseSqlServer(
                    $"Server=localhost;Database=AEXMovies.Testing.Integration.{_appId:N};User=sa;Password=Password$")
                .UseApplicationServiceProvider(sp)
                .Options);
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using (var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<EfDbContext>();
            context.Database.EnsureCreated();
        }

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        using (var serviceScope = Server.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<EfDbContext>();
            context.Database.EnsureDeleted();
        }
    }
}