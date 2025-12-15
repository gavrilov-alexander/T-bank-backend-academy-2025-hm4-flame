using Flames.Core.Variations;
using FluentAssertions;

namespace Flames.Tests.Unit.Variations;

public class HorseshoeVariationTests
{
    [Fact]
    public void Apply_ReturnsCorrectValues()
    {
        var v = new HorseshoeVariation();
        var (x, y) = v.Apply(1.0, 1.0);
        double r = Math.Sqrt(2);
        double expectedX = (1 - 1) * (1 + 1) / r;
        double expectedY = 2 * 1 * 1 / r;
        x.Should().BeApproximately(expectedX, 1e-10);
        y.Should().BeApproximately(expectedY, 1e-10);
    }
}