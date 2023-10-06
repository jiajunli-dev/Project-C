namespace API.Tests.Utility;

[TestClass]
public abstract class TestBase
{
    protected WebApiFactory<Program> _factory;
    protected IServiceProvider _provider;

    [TestInitialize]
    public void Initialize()
    {
        _provider = Startup.ConfigureServices();
        _factory = _provider.GetRequiredService<WebApiFactory<Program>>();
    }
}
