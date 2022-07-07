using AEXMovies.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AEXMovies.Repositories;

public class EfDbContext : IdentityDbContext<User>
{
    public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<MovieActor> MovieActors { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });

        builder.Entity<MovieActor>()
            .HasKey(mg => new { mg.MovieId, mg.ActorId });


        builder.Entity<Genre>().HasIndex(g => g.NormalizedName).IsUnique();
        builder.Entity<Movie>().HasIndex(m => m.Mpa);
    }
}