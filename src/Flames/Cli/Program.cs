using Flames.Cli;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Flames.Cli;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var factory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Information));
        var logger = factory.CreateLogger<Program>();

        try
        {
            var app = new FlameApp(logger);
            return await app.RunAsync(args);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error");
            return 1;
        }
    }
}