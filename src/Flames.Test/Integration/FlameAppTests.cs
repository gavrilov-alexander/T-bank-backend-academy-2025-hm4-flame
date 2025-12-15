using Flames.Cli;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flames.Test.Integration;
public class FlameAppTests
{
    [Fact]
    public async Task FlameApp_RunAsync_WithValidConfig_CreatesImage()
    {
        var logger = NullLogger.Instance;
        var app = new FlameApp(logger);
        var args = new[] { "-f", "swirl:1.0", "-i", "1000", "-w", "100", "-h", "100", "-o", "test_out.png" };

        var result = await app.RunAsync(args);

        result.Should().Be(0);
        File.Exists("test_out.png").Should().BeTrue();
        File.Delete("test_out.png");
    }
}
