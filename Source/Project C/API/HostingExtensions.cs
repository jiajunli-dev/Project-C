using System.Text;

using API.Utility;

using Data.Exceptions;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;

using Destructurama;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
            app.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            app.Services.AddHttpClient();
            app.Services.AddScoped<IClerkClient, ClerkClient>();
            app.Services.AddScoped<ITicketRepository, TicketRepository>();
            app.Services.AddScoped<IPhotoRepository, PhotoRepository>();
            app.Services.AddScoped<IMalfunctionRepository, MalfunctionRepository>();
            app.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            app.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            app.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            app.Services.AddControllers(configure =>
            {
                configure.RespectBrowserAcceptHeader = true;
                configure.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            });
            app.Services.AddEndpointsApiExplorer();
            app.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = JwtBearerDefaults.AuthenticationScheme,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme,Id = JwtBearerDefaults.AuthenticationScheme}
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return app;
        }

        public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder app)
        {
            //if (app.Environment.IsDevelopment())
            //{
            //    const string name = "ProjectCDb";
            //    app.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(name));
            //    using var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(name).Options);
            //    context.Database.EnsureCreated();
            //}
            //else
            //{
            //    string connectionString = app.Configuration["ConnectionStrings:Default"] ?? throw new NullReferenceException("Connection string is not provided in appsettings.json");
            //    app.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
            //    using var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connectionString).Options);
            //    context.Database.Migrate();
            //    context.Database.EnsureCreated();
            //}
            string connectionString = app.Configuration["ConnectionStrings:Default"] ?? throw new MissingValueException("Connection string is not provided in appsettings.json");
            app.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            using var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options);
            context.Database.Migrate();
            context.Database.EnsureCreated();
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
            app.UseCors("AllowReactApp");
            return app;
        }

        public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder app)
        {
            string authority = app.Configuration["Clerk:AuthorityUri"] ?? throw new NullReferenceException("Authority is not provided in appsettings.json");
            string audience = app.Configuration["Clerk:AudienceUri"] ?? throw new NullReferenceException("Audience is not provided in appsettings.json");
            string signingKey = app.Configuration["Clerk:SigningKey"] ?? throw new NullReferenceException("Signing key is not provided in appsettings.json");

            app.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = authority;
                    options.Audience = audience;
                    options.TokenValidationParameters = new()
                    {
                        ValidIssuer = authority,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });
            app.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", builder =>
                {
                    builder
                    .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            return app;
        }
    }
}
