using Flames.Core;
using FluentAssertions;

namespace Flames.Tests.Unit;

public class HistogramTests
{
    [Fact]
    public void AddPoint_IncrementsCountAndColor()
    {
        var h = new Histogram(2, 2);
        h.AddPoint(0, 0, 0.5, 0.6, 0.7);

        h.Counts[0].Should().Be(1);
        h.R[0].Should().Be(0.5);
        h.G[0].Should().Be(0.6);
        h.B[0].Should().Be(0.7);
    }

    [Fact]
    public void Merge_CombinesTwoHistograms()
    {
        var h1 = new Histogram(1, 1);
        var h2 = new Histogram(1, 1);
        h1.AddPoint(0, 0, 0.2, 0.3, 0.4);
        h2.AddPoint(0, 0, 0.6, 0.7, 0.8);

        h1.Merge(h2);

        h1.Counts[0].Should().Be(2);
        h1.R[0].Should().Be(0.8);
        h1.G[0].Should().Be(1.0);
        Math.Round(h1.B[0], 1).Should().Be(1.2);
    }
}