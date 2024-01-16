using Data.Exceptions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        return new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseNpgsql("Host=185.107.90.176; Database=Test; Username=platiumx; Password=Epsilon906!").Options);

        //const string name = "ProjectCDb";
        //return new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(name).Options);
    }
}
