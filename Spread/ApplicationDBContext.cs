namespace Spread;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Country> Country { get; set; }
    public DbSet<Book> Book { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Specify PostgreSQL as the database provider
        optionsBuilder.UseNpgsql("Host=localhost;Database=spread_db;Username=postgres;Password=timppa")
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        // You can define relationships and constraints here (optional)
        modelBuilder.Entity<Country>().ToTable("country");
        modelBuilder.Entity<Book>().ToTable("book")
            .HasOne<Country>(b => b.Country)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CountryId);
    }
}