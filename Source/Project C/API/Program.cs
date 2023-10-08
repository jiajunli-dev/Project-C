using API;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureConfiguration()
    .ConfigureLogger()
    .ConfigureServices()
    .ConfigureDatabase();

var app = builder.Build();
app.ConfigurePipeline().Run();

#pragma warning disable S1118 // Force public for API.Tests to access
public partial class Program { }
#pragma warning restore S1118 // Utility classes should not have public constructors
