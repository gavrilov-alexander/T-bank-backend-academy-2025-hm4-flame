using Flames.Core;
using Flames.Core.Models;

namespace Flames.Cli;

public static class RenderingEngine
{
    public static async Task<Histogram> RenderAsync(RenderConfig config)
    {
        return config.Threads <= 1
            ? new FlameRenderer(config).Render()
            : await RenderParallelAsync(config);
    }

    private static async Task<Histogram> RenderParallelAsync(RenderConfig config)
    {
        var tasks = new List<Task<Histogram>>();
        var baseSeed = config.Seed;
        var itersPer = config.IterationCount / config.Threads;
        var rem = config.IterationCount % config.Threads;

        for (int i = 0; i < config.Threads; i++)
        {
            var localIters = itersPer + (i == config.Threads - 1 ? rem : 0);
            var localConfig = CloneConfig(config, localIters, baseSeed + i * 1000);
            tasks.Add(Task.Run(() => new FlameRenderer(localConfig).Render()));
        }

        var histograms = await Task.WhenAll(tasks);
        var merged = new Histogram(config.Width, config.Height);
        foreach (var h in histograms)
        {
            merged.Merge(h);
        }

        return merged;
    }

    private static RenderConfig CloneConfig(RenderConfig src, int iterationCount, double seed)
    {
        return new RenderConfig
        {
            Width = src.Width,
            Height = src.Height,
            IterationCount = iterationCount,
            OutputPath = src.OutputPath,
            Threads = 1,
            Seed = seed,
            GammaCorrection = src.GammaCorrection,
            Gamma = src.Gamma,
            SymmetryLevel = src.SymmetryLevel,
            Functions = src.Functions.Select(f => new FlameFunction
            {
                Variation = f.Variation,
                Weight = f.Weight,
                Color = f.Color,
                Affine = new AffineParams
                {
                    A = f.Affine.A,
                    B = f.Affine.B,
                    C = f.Affine.C,
                    D = f.Affine.D,
                    E = f.Affine.E,
                    F = f.Affine.F
                }
            }).ToList()
        };
    }
}