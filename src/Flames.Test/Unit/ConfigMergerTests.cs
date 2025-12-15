using Flames.Cli;
using Flames.Core.Models;
using FluentAssertions;

namespace Flames.Test.Unit;
public class ConfigMergerTests
{
    [Fact]
    public void ConfigMerger_Merge_CliOverridesJson()
    {
        var jsonConfig = new RenderConfig { Width = 640, IterationCount = 5000 };
        var cliInput = new CliInput { Width = 800, IterationCount = 10000 };

        var result = ConfigMerger.Merge(jsonConfig, cliInput);

        result.Width.Should().Be(800);
        result.IterationCount.Should().Be(10000);
    }
}
