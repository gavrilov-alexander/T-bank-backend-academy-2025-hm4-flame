using Flames.Core.Variations;
using FluentAssertions;

namespace Flames.Tests.Unit.Variations;

public class LinearVariationTests
{
    [Fact]
    public void Apply_ReturnsSameCoordinates()
    {
        var v = new LinearVariation();
        var (x, y) = v.Apply(1.5, -2.3);
        x.Should().Be(1.5);
        y.Should().Be(-2.3);
    }
}