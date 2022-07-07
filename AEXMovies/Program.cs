using System.Text.Json.Serialization;
using AEXMovies;
using AEXMovies.Configuration;
using AEXMovies.Models;
using AEXMovies.Repositories;
using AEXMovies.Repositories.ActorRepository;
using AEXMovies.Repositories.GenreRepository;
using AEXMovies.Repositories.MovieRepository;
using AEXMovies.Repositories.RefreshTokenRepository;
using AEXMovies.Services.ActorService;
using AEXMovies.Services.AssetsLoaderService;
using AEXMovies.Services.AuthService;
using AEXMovies.Services.Dtos;
using AEXMovies.Services.GenreService;
using AEXMovies.Services.MovieService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting();
builder.Services.AddMvc()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.AddControllers();


var jwtConfig = builder.Configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>() ?? new JwtConfig();
builder.Services.AddSingleton(jwtConfig);
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Audience = jwtConfig.Audience;
        options.ClaimsIssuer = jwtConfig.Issuer;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = jwtConfig.Audience,
            ValidateAudience = true,
            ValidAlgorithms = new[] { "HS256" },
            IssuerSigningKey = jwtConfig.GetSecurityKey()
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .Build();
});

// database connection
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<EfDbContext>(options => { options.UseSqlServer(connectionString); });

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<EfDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(DtoAutoMapperProfile));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


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


var command = args.Length == 0 ? string.Empty : args[0];

if (command == "populate")
{
    Console.WriteLine("Populating database...");
    await Populate.Run(app);
}
else
{
    app.Run();
}


// this is a required workaround for testing: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0&viewFallbackFrom=aspnetcore-3.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program
{
}