using Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext : DbContext
{
  // todo Add DbSets
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<TicketPhoto> TicketPhotos { get; set; }
    public DbSet<Malfunction> Malfunctions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }

    /// <summary>
    /// Launches DbContext with the provided DbContextOptions
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public AppDbContext()
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    base.OnConfiguring(optionsBuilder);
    }
}
