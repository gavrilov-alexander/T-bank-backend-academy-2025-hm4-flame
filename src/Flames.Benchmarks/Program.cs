using BenchmarkDotNet.Running;
using Flames.Tests.Benchmarks;

namespace Flames.Benchmarks;

internal class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<RendererBenchmark>();
    }
}
