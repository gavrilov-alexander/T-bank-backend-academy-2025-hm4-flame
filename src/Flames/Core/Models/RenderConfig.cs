using System.Text.Json.Serialization;

namespace Flames.Core.Models;

public class RenderConfig
{
    [JsonPropertyName("iteration_count")]
    public int IterationCount { get; set; } = 2500;

    [JsonPropertyName("output_path")]
    public string OutputPath { get; set; } = "result.png";

    [JsonPropertyName("threads")]
    public int Threads { get; set; } = 1;

    [JsonPropertyName("seed")]
    public double Seed { get; set; } = 5.1234;

    [JsonPropertyName("gamma_correction")]
    public bool GammaCorrection { get; set; } = false;

    [JsonPropertyName("gamma")]
    public double Gamma { get; set; } = 2.2;

    [JsonPropertyName("symmetry_level")]
    public int SymmetryLevel { get; set; } = 1;

    [JsonPropertyName("size")]
    public JsonSize? Size { get; set; }

    [JsonPropertyName("functions")]
    public List<JsonFunction>? FunctionsJson { get; set; }

    [JsonPropertyName("affine_params")]
    public object? AffineParamsRaw { get; set; }

    [JsonIgnore]
    public int Width { get; set; } = 1920;
    [JsonIgnore]
    public int Height { get; set; } = 1080;

    [JsonIgnore]
    public List<FlameFunction> Functions { get; set; } = new();

    [JsonIgnore]
    public List<(string name, double weight)>? CliFunctionSpecs { get; set; }

    [JsonIgnore]
    public List<AffineParams>? CliAffineParams { get; set; }
}