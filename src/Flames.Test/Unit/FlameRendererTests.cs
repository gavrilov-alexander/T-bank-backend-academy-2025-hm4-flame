using Flames.Core;
using Flames.Core.Models;
using FluentAssertions;
using Xunit;

namespace Flames.Tests.Unit;

public class FlameRendererTests
{
    [Fact]
    public void Render_WithLinearFunction_ReturnsHistogramWithValidPoints()
    {
        var config = new RenderConfig
        {
            Width = 200,
            Height = 150,
            IterationCount = 1000,
            Functions = new List<FlameFunction>
            {
                new()
                {
                    Variation = "linear",
                    Weight = 1.0,
                    Affine = new AffineParams { A = 0.5f, E = 0.5f },
                    Color = (0.8, 0.2, 0.6)
                }
            }
        };

        var renderer = new FlameRenderer(config);

        var histogram = renderer.Render();

        histogram.Should().NotBeNull();
        histogram.Width.Should().Be(200);
        histogram.Height.Should().Be(150);

        var nonZero = histogram.Counts.Count(c => c > 0);
        nonZero.Should().BeGreaterThan(0);
        nonZero.Should().BeLessThanOrEqualTo(1000);
    }

    [Fact]
    public void Render_WithSwirlAndSinusoidal_CoversSignificantArea()
    {
        var config = new RenderConfig
        {
            Width = 300,
            Height = 169,
            IterationCount = 50000,
            Functions = new List<FlameFunction>
            {
                new() { Variation = "swirl", Weight = 0.5, Affine = new AffineParams { A = 0.8, B = -0.1, C = 0.6, D = 0.1, E = 0.8, F = 0.4 }, Color = (1, 0, 0) },
                new() { Variation = "sinusoidal", Weight = 0.5, Affine = new AffineParams { A = 0.7, B = 0.2, C = -0.5, D = -0.2, E = 0.7, F = -0.3 }, Color = (0, 1, 0) }
            }
        };

        var histogram = new FlameRenderer(config).Render();
        var nonZero = histogram.Counts.Count(c => c > 0);
        nonZero.Should().BeGreaterThan(1000);
    }

    [Fact]
    public void Render_HistogramCountsMatchIterationCountApproximately()
    {
        const int iterations = 10000;
        var config = new RenderConfig
        {
            Width = 400,
            Height = 225,
            IterationCount = iterations,
            Functions = new List<FlameFunction>
            {
                new() { Variation = "linear", Weight = 1.0, Affine = new AffineParams { A = 0.9, E = 0.9, C = 0.1, F = 0.1 }, Color = (0.5, 0.5, 0.5) }
            }
        };

        var histogram = new FlameRenderer(config).Render();
        var totalCount = histogram.Counts.Sum();
        totalCount.Should().BeInRange(iterations - 100, iterations);
    }

    [Fact]
    public void Render_UsesCorrectColorBlending()
    {
        var config = new RenderConfig
        {
            Width = 100,
            Height = 100,
            IterationCount = 100,
            Functions = new List<FlameFunction>
            {
                new() { Variation = "linear", Weight = 1.0, Affine = new AffineParams(), Color = (1.0, 0.0, 0.0) }
            }
        };

        var histogram = new FlameRenderer(config).Render();
        var totalR = histogram.R.Sum();
        var totalG = histogram.G.Sum();
        var totalB = histogram.B.Sum();

        totalR.Should().BeGreaterThan(0);
        totalG.Should().BeApproximately(0, 1e-5);
        totalB.Should().BeApproximately(0, 1e-5);
    }
}