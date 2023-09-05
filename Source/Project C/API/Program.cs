using API;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureConfiguration()
    .ConfigureServices()
    .ConfigureDatabase();

var app = builder.Build();
app.ConfigurePipeline().Run();
