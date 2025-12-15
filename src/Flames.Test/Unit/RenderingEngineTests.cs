using Flames.Cli;
using Flames.Core.Models;
using FluentAssertions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Flames.Test.Unit;
public class RenderingEngineTests
{
    [Fact]
    public async Task RenderingEngine_RenderAsync_ReturnsValidHistograms()
    {
        var config = new RenderConfig
        {
            Width = 100,
            Height = 100,
            IterationCount = 10000,
            Threads = 1,
            Functions = new List<FlameFunction>
        {
            new() { Variation = "swirl", Weight = 0.5, Affine = new AffineParams { A = 0.8, B = -0.1, C = 0.6, D = 0.1, E = 0.8, F = 0.4 }, Color = (1, 0, 0) }
        }
        };

        var single = await RenderingEngine.RenderAsync(config);
        single.Should().NotBeNull();
        single.Width.Should().Be(100);
        single.Height.Should().Be(100);
        single.Counts.Sum().Should().BeGreaterThan(0);

        config.Threads = 2;
        var multi = await RenderingEngine.RenderAsync(config);
        multi.Should().NotBeNull();
        multi.Width.Should().Be(100);
        multi.Height.Should().Be(100);
        multi.Counts.Sum().Should().BeGreaterThan(0);
    }
}
