using Data.Models;
using Data.Repositories;

using Destructurama;

using Microsoft.EntityFrameworkCore;

using Serilog;

using AppDbContext = Data.AppDbContext;
namespace API
{
    public static class HostingExtensions
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder app)
        {
            app.Configuration.AddEnvironmentVariables();
            app.Configuration.AddJsonFile("appsettings.json");
            if (app.Environment.IsDevelopment())
                app.Configuration.AddJsonFile("appsettings.Development.json");

            return app;
        }

        public static WebApplicationBuilder ConfigureLogger(this WebApplicationBuilder app)
        {
            Log.Logger = new LoggerConfiguration().Destructure.UsingAttributes()
                                                  .ReadFrom.Configuration(app.Configuration)
                                                  .Enrich.WithProperty("Name", "API")
                                                  .Enrich.WithProperty("Environment", app.Configuration["Environment"])
                                                  .WriteTo.Console()
                                                  .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                                                  .CreateLogger();

            app.Host.UseSerilog();

            return app;
        }

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder app)
        {
            app.Services.AddScoped<TicketRepository>();
            app.Services.AddScoped<PhotoRepository>();
            app.Services.AddScoped<MalfunctionRepository>();
            app.Services.AddScoped<UserRepository>();
            app.Services.AddScoped<CustomerRepository>();
            app.Services.AddScoped<EmployeeRepository>();
            app.Services.AddScoped<DepartmentRepository>();

            app.Services.AddControllers();
            app.Services.AddEndpointsApiExplorer();
            app.Services.AddSwaggerGen();

            return app;
        }

        public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder app)
        {
            if (app.Environment.IsDevelopment())
            {
                const string name = "ProjectCDb";
                app.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(name));
                using var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(name).Options);
                context.Database.EnsureCreated();
            }
            else
            {
                string connectionString = app.Configuration["ConnectionStrings:Default"] ?? throw new NullReferenceException("Connection string is not provided in appsettings.json");
                app.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
                using var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connectionString).Options);
                context.Database.Migrate();
                context.Database.EnsureCreated();
            }

            return app;
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
