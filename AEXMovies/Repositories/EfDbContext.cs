using AEXMovies.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AEXMovies.Repositories;

public class EfDbContext : IdentityDbContext<User>
{
    public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; } = null!;
    public virtual DbSet<Genre> Genres { get; set; } = null!;
    public virtual DbSet<MovieGenre> MovieGenres { get; set; } = null!;
    public virtual DbSet<Actor> Actors { get; set; } = null!;
    public virtual DbSet<MovieActor> MovieActors { get; set; } = null!;
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;


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