using Flames.Core;
using Flames.Core.Models;
using Microsoft.Extensions.Logging;

namespace Flames.Cli;

public class FlameApp
{
    private readonly ILogger _logger;

    public FlameApp(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<int> RunAsync(string[] args)
    {
        var cliInput = CliParser.Parse(args);
        var config = LoadConfig(cliInput);
        ValidateConfig(config);

        _logger.LogInformation("Starting render: {W}x{H}, {Iters} iters, {Threads} threads",
            config.Width, config.Height, config.IterationCount, config.Threads);

        var histogram = await RenderingEngine.RenderAsync(config);
        var pixelData = ToneMapper.ApplyToneMapping(histogram, config.GammaCorrection, config.Gamma);
        await ImageSaver.SavePngAsync(pixelData, config.Width, config.Height, config.OutputPath);

        _logger.LogInformation("Saved to {Path}", config.OutputPath);
        return 0;
    }

    private RenderConfig LoadConfig(CliInput cli)
    {
        if (cli.ConfigPath != null)
        {
            var jsonConfig = ConfigLoader.Load(cli.ConfigPath);
            return ConfigMerger.Merge(jsonConfig, cli);
        }

        if (cli.FunctionSpecs != null)
        {
            return ConfigMerger.Merge(new RenderConfig(), cli);
        }

        var config = new RenderConfig
        {
            Functions = new List<FlameFunction> { new() { Variation = "linear", Weight = 1.0 } }
        };
        return ConfigMerger.Merge(config, cli);
    }

    private static void ValidateConfig(RenderConfig config)
    {
        if (config.Width <= 0 || config.Width > 10000)
        {
            throw new ArgumentException("Invalid width");
        }

        if (config.Height <= 0 || config.Height > 10000)
        {
            throw new ArgumentException("Invalid height");
        }

        if (config.IterationCount <= 0)
        {
            throw new ArgumentException("Iteration count must be > 0");
        }

        if (config.Threads <= 0)
        {
            throw new ArgumentException("Threads must be > 0");
        }

        if (config.SymmetryLevel < 1)
        {
            throw new ArgumentException("Symmetry level >= 1");
        }

        if (config.Gamma <= 0)
        {
            throw new ArgumentException("Gamma > 0");
        }

        if (config.Functions == null || config.Functions.Count == 0)
        {
            config.Functions = new List<FlameFunction> { new() { Variation = "linear", Weight = 1.0 } };
        }

        foreach (var f in config.Functions)
        {
            if (string.IsNullOrWhiteSpace(f.Variation))
            {
                throw new ArgumentException("Function name required");
            }

            if (f.Weight < 0)
            {
                throw new ArgumentException("Weight >= 0");
            }
        }
    }
}