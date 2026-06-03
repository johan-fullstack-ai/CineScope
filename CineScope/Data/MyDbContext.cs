using Microsoft.EntityFrameworkCore;
using CineScope.Models;

namespace CineScope.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { } // Constructor with Depency Injection for DbContextOptions. Note that EF doesn't support primary constructor.

    public DbSet<Movie> Movies { get; set; } // Create a DBSet for the Movies table in the database.

    public DbSet<Favorite> Favorites { get; set; } // Favorites entity set


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<ApplicationUser>(); // Ignore the ApplicationUser entity to prevent EF from trying to create a table for it.

        _ = modelBuilder.Entity<Movie>().HasData(
            new Movie { Id = 1, Title = "Inception", Genre = "Sci-Fi", ReleaseYear = "2010", Rating = "8.8", Duration = "148", PosterUrl = "https://image.tmdb.org/t/p/w500/edv5CZvWj09upOsy2Y6IwDhK8bt.jpg", Description = "A skilled thief enters dreams to steal secrets." },
            new Movie { Id = 2, Title = "The Dark Knight", Genre = "Action", ReleaseYear = "2008", Rating = "9.0", Duration = "152", PosterUrl = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg", Description = "Batman faces the Joker in Gotham City." },
            new Movie { Id = 3, Title = "Interstellar", Genre = "Sci-Fi", ReleaseYear = "2014", Rating = "8.6", Duration = "169", PosterUrl = "https://image.tmdb.org/t/p/w500/rAiYTfKGqDCRIIqo664sY9XZIvQ.jpg", Description = "A team travels through a wormhole to save humanity." }
        );
    }
}