using Flames.Core.Models;

namespace Flames.Cli;

public static class ConfigMerger
{
    public static RenderConfig Merge(RenderConfig json, CliInput cli)
    {
        bool cliWidthSet = cli.Width != 1920;
        bool cliHeightSet = cli.Height != 1080;
        bool cliIterSet = cli.IterationCount != 2500;
        bool cliOutputSet = cli.OutputPath != "result.png";
        bool cliThreadsSet = cli.Threads != 1;
        bool cliSeedSet = cli.Seed != 5.1234;
        bool cliGammaSet = cli.Gamma != 2.2;
        bool cliSymSet = cli.SymmetryLevel != 1;

        var config = new RenderConfig
        {
            Width = cliWidthSet ? cli.Width : json.Width,
            Height = cliHeightSet ? cli.Height : json.Height,
            IterationCount = cliIterSet ? cli.IterationCount : json.IterationCount,
            OutputPath = cliOutputSet ? cli.OutputPath : json.OutputPath,
            Threads = cliThreadsSet ? cli.Threads : json.Threads,
            Seed = cliSeedSet ? cli.Seed : json.Seed,
            GammaCorrection = cli.GammaCorrection || json.GammaCorrection,
            Gamma = cliGammaSet ? cli.Gamma : json.Gamma,
            SymmetryLevel = cliSymSet ? cli.SymmetryLevel : json.SymmetryLevel,
            Functions = cli.FunctionSpecs != null
                ? BuildFunctionsFromSpecs(cli.FunctionSpecs, cli.AffineParams, cli.Seed)
                : json.Functions
        };

        var rand = new Random(BitConverter.ToInt32(BitConverter.GetBytes(config.Seed)));
        foreach (var f in config.Functions)
        {
            if (f.Color == (0.0, 0.0, 0.0))
            {
                f.Color = (rand.NextDouble(), rand.NextDouble(), rand.NextDouble());
            }
        }

        return config;
    }

    private static List<FlameFunction> BuildFunctionsFromSpecs(
        List<(string name, double weight)> specs,
        List<AffineParams>? affines,
        double seed)
    {
        if (specs.Count == 0)
        {
            specs.Add(("linear", 1.0));
        }

        affines ??= new List<AffineParams>();
        while (affines.Count < specs.Count)
        {
            affines.Add(new AffineParams());
        }

        var rand = new Random(BitConverter.ToInt32(BitConverter.GetBytes(seed)));
        return specs.Select((spec, i) => new FlameFunction
        {
            Variation = spec.name,
            Weight = spec.weight,
            Affine = affines[i],
            Color = (rand.NextDouble(), rand.NextDouble(), rand.NextDouble())
        }).ToList();
    }
}