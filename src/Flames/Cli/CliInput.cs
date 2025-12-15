using Flames.Core.Models;

namespace Flames.Cli;
public class CliInput
{
    public string? ConfigPath { get; set; }
    public int Width { get; set; } = 1920;
    public int Height { get; set; } = 1080;
    public double Seed { get; set; } = 5.1234;
    public int IterationCount { get; set; } = 2500;
    public string OutputPath { get; set; } = "result.png";
    public int Threads { get; set; } = 1;
    public List<(string name, double weight)>? FunctionSpecs { get; set; }
    public List<AffineParams>? AffineParams { get; set; }
    public bool GammaCorrection { get; set; } = false;
    public double Gamma { get; set; } = 2.2;
    public int SymmetryLevel { get; set; } = 1;
}