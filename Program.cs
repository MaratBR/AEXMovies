using System.Text.Json.Serialization;
using AEXMovies;
using AEXMovies.Models;
using AEXMovies.Repositories;
using AEXMovies.Repositories.ActorRepository;
using AEXMovies.Repositories.GenreRepository;
using AEXMovies.Repositories.MovieRepository;
using AEXMovies.Services.ActorService;
using AEXMovies.Services.AssetsLoaderService;
using AEXMovies.Services.AuthService;
using AEXMovies.Services.Dtos;
using AEXMovies.Services.GenreService;
using AEXMovies.Services.MovieService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// TODO: change distributed cache "strategy" based on configuration (i.e. redis/sqlserver/memory)
builder.Services.AddDistributedMemoryCache();

builder.Services.AddRouting();
builder.Services.AddMvc()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddControllers();

// authentication/authorization
// TODO: implement 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
    });
builder.Services.AddAuthorization();

// database connection
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<EfDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<EfDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(DtoAutoMapperProfile));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddTransient<IAssetsLoaderService, AssetsLoaderService>();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


string command = args.Length == 0 ? "serve" : args[0];

if (command == "populate")
{
    Console.WriteLine("Populating database...");
    await Populate.Run(app);
}
else if (command == "serve")
{
    app.Run();
}
else
{
    Console.Error.WriteLine($"Unknown command: {command}");
    Environment.Exit(1);
}


// this is a required workaround for testing: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0&viewFallbackFrom=aspnetcore-3.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program
{
}