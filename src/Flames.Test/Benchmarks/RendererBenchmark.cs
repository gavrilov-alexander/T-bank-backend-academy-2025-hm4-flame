using BenchmarkDotNet.Attributes;
using Flames.Cli;
using Flames.Core;
using Flames.Core.Models;

namespace Flames.Tests.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class RendererBenchmark
{
    private RenderConfig _config = new()
    {
        Width = 600,
        Height = 338,
        IterationCount = 50_000_000,
        Functions = new List<FlameFunction>
        {
            new() { Variation = "swirl", Weight = 0.5f, Affine = new AffineParams { A = 0.8f, B = -0.1f, C = 0.6f, D = 0.1f, E = 0.8f, F = 0.4f }, Color = (1, 0, 0) },
            new() { Variation = "sinusoidal", Weight = 0.5f, Affine = new AffineParams { A = 0.7f, B = 0.2f, C = -0.5f, D = -0.2f, E = 0.7f, F = -0.3f }, Color = (0, 1, 0) },
            new() { Variation = "spherical", Weight = 0.5f, Affine = new AffineParams { A = 0.9f, B = 0.0f, C = 0.1f, D = 0.0f, E = 0.9f, F = -0.1f }, Color = (0, 0, 1) }
        }
    };

    [Benchmark]
    public Histogram SingleThread()
    {
        return new FlameRenderer(_config).Render();
    }

    [Benchmark]
    public async Task<Histogram> MultiThread()
    {
        _config.Threads = 8;
        return await RenderingEngine.RenderAsync(_config);
    }
}