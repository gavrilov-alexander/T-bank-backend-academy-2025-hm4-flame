using Flames.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using FluentAssertions;
using Flames.Core.Models;

namespace Flames.Tests.Integration;

public class EndToEndRenderTests
{
    [Fact]
    public void RenderLinearFunction_ProducesValidImage()
    {
        var config = new RenderConfig
        {
            Width = 100,
            Height = 100,
            IterationCount = 1000,
            Functions = new List<FlameFunction>
            {
                new() { Variation = "linear", Weight = 1.0, Affine = new AffineParams(), Color = (1, 0, 0) }
            }
        };

        var renderer = new FlameRenderer(config);
        var histogram = renderer.Render();
        var pixels = ToneMapper.ApplyToneMapping(histogram, false, 2.2);

        pixels.Should().NotBeNull();
        pixels.Length.Should().Be(100 * 100 * 3);

        var hasColor = pixels.Any(b => b > 0);
        hasColor.Should().BeTrue();
    }
}