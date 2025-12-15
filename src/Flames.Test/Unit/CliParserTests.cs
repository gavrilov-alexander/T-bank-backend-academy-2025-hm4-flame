using Flames.Cli;
using Flames.Core.Models;
using FluentAssertions;
using Xunit;

namespace Flames.Tests.Unit;

public class CliParserTests
{
    [Fact]
    public void Parse_WithValidCli_ReturnsCorrectInput()
    {
        var args = new[] { "-f", "swirl:1.0", "-i", "1000", "-w", "800", "-h", "450" };

        var input = CliParser.Parse(args);

        input.FunctionSpecs.Should().HaveCount(1);
        input.FunctionSpecs![0].name.Should().Be("swirl");
        input.FunctionSpecs[0].weight.Should().Be(1.0);
        input.IterationCount.Should().Be(1000);
        input.Width.Should().Be(800);
        input.Height.Should().Be(450);
    }

    [Fact]
    public void LoadJsonConfig_ReturnsCorrectConfig()
    {
        var json = @"{
            ""size"": { ""width"": 640, ""height"": 360 },
            ""iteration_count"": 5000,
            ""functions"": [{ ""name"": ""linear"", ""weight"": 1.0 }],
            ""affine_params"": { ""a"": 0.5, ""b"": 0.0, ""c"": 0.0, ""d"": 0.0, ""e"": 0.5, ""f"": 0.0 }
        }";
        const string path = "test_config.json";
        File.WriteAllText(path, json);

        try
        {
            var config = ConfigLoader.Load(path);

            config.Width.Should().Be(640);
            config.Height.Should().Be(360);
            config.IterationCount.Should().Be(5000);
            config.Functions.Should().HaveCount(1);
            config.Functions[0].Variation.Should().Be("linear");
            config.Functions[0].Weight.Should().Be(1.0);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Parse_InvalidAffineParams_Throws()
    {
        var args = new[] { "-ap", "1,2,3", "-f", "swirl:1.0" };
        FluentActions.Invoking(() => CliParser.Parse(args))
                     .Should().Throw<ArgumentException>();
    }
}