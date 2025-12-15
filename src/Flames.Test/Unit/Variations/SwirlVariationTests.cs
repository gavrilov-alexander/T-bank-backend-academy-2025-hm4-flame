using Flames.Core.Variations;
using FluentAssertions;

namespace Flames.Tests.Unit.Variations;

public class SwirlVariationTests
{
    [Fact]
    public void Apply_AppliesSwirlTransformation()
    {
        var v = new SwirlVariation();
        var (x, y) = v.Apply(1.0, 0.0);
        double r2 = 1.0;
        double expectedX = Math.Sin(r2);
        double expectedY = Math.Cos(r2);
        x.Should().BeApproximately(expectedX, 1e-10);
        y.Should().BeApproximately(expectedY, 1e-10);
    }
}