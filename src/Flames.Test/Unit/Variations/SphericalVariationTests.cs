using Flames.Core.Variations;
using FluentAssertions;

namespace Flames.Tests.Unit.Variations;

public class SphericalVariationTests
{
    [Fact]
    public void Apply_InvertsCoordinates()
    {
        var v = new SphericalVariation();
        var (x, y) = v.Apply(2.0, 0.0);
        x.Should().BeApproximately(0.5, 1e-10);
        y.Should().Be(0.0);
    }

    [Fact]
    public void Apply_ReturnsZero_WhenInputIsZero()
    {
        var v = new SphericalVariation();
        var (x, y) = v.Apply(0, 0);
        x.Should().Be(0);
        y.Should().Be(0);
    }
}