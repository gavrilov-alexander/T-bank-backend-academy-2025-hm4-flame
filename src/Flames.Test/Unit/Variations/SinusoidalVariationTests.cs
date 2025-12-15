using Flames.Core.Variations;
using FluentAssertions;

namespace Flames.Tests.Unit.Variations;

public class SinusoidalVariationTests
{
    [Fact]
    public void Apply_ReturnsSinOfCoordinates()
    {
        var v = new SinusoidalVariation();
        var (x, y) = v.Apply(Math.PI / 2, 0);
        x.Should().BeApproximately(1.0, 1e-10);
        y.Should().BeApproximately(0.0, 1e-10);
    }
}