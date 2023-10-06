using System.Data.Common;

using Data;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace API.Tests.Utility;

public class WebApiFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            if (services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)) is ServiceDescriptor dbContextDescriptor)
                services.Remove(dbContextDescriptor);
            if (services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection)) is ServiceDescriptor dbConnectionDescriptor)
                services.Remove(dbConnectionDescriptor);

            services.AddDbContext<AppDbContext>((container, options) => options.UseInMemoryDatabase("ProjectCDb"));
        });

        builder.UseEnvironment("Development");
    }
}