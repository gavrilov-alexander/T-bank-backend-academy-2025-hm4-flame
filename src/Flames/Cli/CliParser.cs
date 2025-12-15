using Flames.Core.Models;
using System.Globalization;

namespace Flames.Cli;

public static class CliParser
{
    public static CliInput Parse(string[] args)
    {
        var input = new CliInput();
        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            string? value = i + 1 < args.Length ? args[i + 1] : null;
            bool HasValue() => value != null && !value.StartsWith('-');

            if (arg == "--config" && HasValue()) { input.ConfigPath = value!; i++; }
            else if ((arg == "-w" || arg == "--width") && HasValue()) { input.Width = int.Parse(value!, CultureInfo.InvariantCulture); i++; }
            else if ((arg == "-h" || arg == "--height") && HasValue()) { input.Height = int.Parse(value!, CultureInfo.InvariantCulture); i++; }
            else if (arg == "--seed" && HasValue()) { input.Seed = double.Parse(value!, CultureInfo.InvariantCulture); i++; }
            else if ((arg == "-i" || arg == "--iteration-count") && HasValue()) { input.IterationCount = int.Parse(value!, CultureInfo.InvariantCulture); i++; }
            else if ((arg == "-o" || arg == "--output-path") && HasValue()) { input.OutputPath = value!; i++; }
            else if ((arg == "-t" || arg == "--threads") && HasValue()) { input.Threads = int.Parse(value!, CultureInfo.InvariantCulture); i++; }
            else if ((arg == "-ap" || arg == "--affine-params") && HasValue()) { input.AffineParams = ParseAffineParams(value!); i++; }
            else if ((arg == "-f" || arg == "--functions") && HasValue()) { input.FunctionSpecs = ParseFunctions(value!); i++; }
            else if (arg == "-g" || arg == "--gamma-correction") { input.GammaCorrection = true; }
            else if (arg == "--gamma" && HasValue()) { input.Gamma = double.Parse(value!, CultureInfo.InvariantCulture); i++; }
            else if ((arg == "-s" || arg == "--symmetry-level") && HasValue()) { input.SymmetryLevel = int.Parse(value!, CultureInfo.InvariantCulture); i++; }
        }
        return input;
    }

    private static List<(string name, double weight)> ParseFunctions(string spec)
    {
        var result = new List<(string, double)>();
        foreach (var part in spec.Split(','))
        {
            var kv = part.Split(':');
            if (kv.Length != 2)
            {
                throw new ArgumentException($"Invalid function: {part}");
            }

            result.Add((kv[0].Trim().ToLowerInvariant(), double.Parse(kv[1], CultureInfo.InvariantCulture)));
        }
        return result;
    }

    private static List<AffineParams> ParseAffineParams(string spec)
    {
        var result = new List<AffineParams>();
        foreach (var group in spec.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = group.Split(',', StringSplitOptions.TrimEntries);
            if (parts.Length != 6)
            {
                throw new ArgumentException("Affine params must be a,b,c,d,e,f");
            }

            result.Add(new AffineParams
            {
                A = double.Parse(parts[0], CultureInfo.InvariantCulture),
                B = double.Parse(parts[1], CultureInfo.InvariantCulture),
                C = double.Parse(parts[2], CultureInfo.InvariantCulture),
                D = double.Parse(parts[3], CultureInfo.InvariantCulture),
                E = double.Parse(parts[4], CultureInfo.InvariantCulture),
                F = double.Parse(parts[5], CultureInfo.InvariantCulture)
            });
        }
        return result;
    }
}