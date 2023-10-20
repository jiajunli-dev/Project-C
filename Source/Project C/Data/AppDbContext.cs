using Data.Interfaces;
using Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Malfunction> Malfunctions { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    /// <summary>
    /// Launches DbContext with the provided DbContextOptions
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public AppDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries()
            .Where(entry => entry is ICreatable && (entry.State == EntityState.Added || entry.State == EntityState.Modified));

        var now = DateTime.UtcNow;
        foreach (var entity in entities)
        {
            if (entity is ICreatable trackable)
            {
                if (entity.State == EntityState.Added)
                    trackable.CreatedAt = now;
                trackable.UpdatedAt = now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
