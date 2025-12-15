using Flames.Core.Models;
using System.Text.Json;

namespace Flames.Cli;

public static class ConfigLoader
{
    public static RenderConfig Load(string path)
    {
        var json = File.ReadAllText(path);
        var opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
        var raw = JsonSerializer.Deserialize<RenderConfig>(json, opts) ?? new RenderConfig();

        if (raw.Size != null)
        {
            raw.Width = raw.Size.Width;
            raw.Height = raw.Size.Height;
        }

        var functions = new List<FlameFunction>();
        var rand = new Random(BitConverter.ToInt32(BitConverter.GetBytes(raw.Seed)));
        var jsonFuncs = raw.FunctionsJson ?? new List<JsonFunction>();
        var affines = ParseAffineParamsFromRaw(raw.AffineParamsRaw, jsonFuncs.Count, opts);

        for (int i = 0; i < jsonFuncs.Count; i++)
        {
            functions.Add(new FlameFunction
            {
                Variation = jsonFuncs[i].Name,
                Weight = jsonFuncs[i].Weight,
                Affine = affines[i],
                Color = (rand.NextDouble(), rand.NextDouble(), rand.NextDouble())
            });
        }

        raw.Functions = functions;
        return raw;
    }

    private static List<AffineParams> ParseAffineParamsFromRaw(object? raw, int funcCount, JsonSerializerOptions opts)
    {
        var result = new List<AffineParams>();
        if (raw is JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                var single = JsonSerializer.Deserialize<AffineParams>(element, opts)!;
                for (int i = 0; i < funcCount; i++)
                {
                    result.Add(single);
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    result.Add(JsonSerializer.Deserialize<AffineParams>(item, opts)!);
                }
            }
        }

        while (result.Count < funcCount)
        {
            result.Add(new AffineParams());
        }

        return result;
    }
}