using System.Diagnostics;
using System.Text;

using Data;

using Microsoft.EntityFrameworkCore;

namespace API.Tests;

[TestClass]
public class GeneralTests : TestBase
{
    [TestMethod]
    public void CreateClassDiagram()
    {
        using var context = new DesignTimeDbContextFactory().CreateDbContext(Array.Empty<string>());
        var path = Path.GetTempFileName() + ".dgml";
        File.WriteAllText(path, context.AsDgml(), Encoding.UTF8);
        var startInfo = new ProcessStartInfo(path)
        {
            UseShellExecute = true,
        };
        Process.Start(startInfo);

        Assert.IsTrue(true);
    }
}
