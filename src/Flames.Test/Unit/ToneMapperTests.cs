using Flames.Core;
using FluentAssertions;

namespace Flames.Tests.Unit;

public class ToneMapperTests
{
    [Fact]
    public void ApplyToneMapping_WithGammaCorrection_ProducesCorrectOutput()
    {
        var h = new Histogram(1, 1);
        h.AddPoint(0, 0, 1.0, 0.5, 0.0);
        h.AddPoint(0, 0, 1.0, 0.5, 0.0);

        var pixels = ToneMapper.ApplyToneMapping(h, gammaCorrection: true, gamma: 2.2);

        pixels[0].Should().BeGreaterThan(200);
        pixels[1].Should().BeGreaterThan(100);
        pixels[2].Should().Be(0);
    }
}