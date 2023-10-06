namespace API.Tests.Utility;

public static class Startup
{
    public static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection().AddTransient<WebApiFactory<Program>>();
        return services.BuildServiceProvider();
    }
}