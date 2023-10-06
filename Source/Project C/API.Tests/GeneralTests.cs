using System.Diagnostics;
using System.Text;

using API.Tests.Utility;

using Data;

using Microsoft.EntityFrameworkCore;

namespace API.Tests;

[TestClass]
public class GeneralTests : TestBase
{
  [TestMethod]
  public void CreateClassDiagram()
  {
    // Arrange
    using var context = new DesignTimeDbContextFactory().CreateDbContext(Array.Empty<string>());
    var path = Path.GetTempFileName() + ".dgml";
    File.WriteAllText(path, context.AsDgml(), Encoding.UTF8);
    var startInfo = new ProcessStartInfo(path)
    {
      UseShellExecute = true,
    };
    Process.Start(startInfo);
  }
}