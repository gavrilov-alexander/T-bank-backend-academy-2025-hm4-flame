using Flames.Core.Models;
using Flames.Core.Variations;

namespace Flames.Core;

public class FlameRenderer
{
    private readonly RenderConfig _config;
    private readonly List<FlameFunction> _functions;
    private readonly Dictionary<string, IVariation> _variations;
    private readonly Random _random;

    public FlameRenderer(RenderConfig config)
    {
        _config = config;
        _random = new Random(BitConverter.ToInt32(BitConverter.GetBytes(_config.Seed)));
        _variations = new Dictionary<string, IVariation>(StringComparer.OrdinalIgnoreCase)
        {
            ["linear"] = new LinearVariation(),
            ["sinusoidal"] = new SinusoidalVariation(),
            ["spherical"] = new SphericalVariation(),
            ["swirl"] = new SwirlVariation(),
            ["horseshoe"] = new HorseshoeVariation()
        };

        _functions = new List<FlameFunction>(_config.Functions);
        if (_functions.Count == 0)
        {
            var f = new FlameFunction { Color = GenerateColor() };
            _functions.Add(f);
        }

        foreach (var f in _functions)
        {
            if (f.Color == (0.0, 0.0, 0.0))
            {
                f.Color = GenerateColor();
            }
        }
    }

    private (double R, double G, double B) GenerateColor() =>
        (_random.NextDouble(), _random.NextDouble(), _random.NextDouble());

    public Histogram Render()
    {
        const double XMIN = -1.777;
        const double XMAX = 1.777;
        const double YMIN = -1.0;
        const double YMAX = 1.0;

        var histogram = new Histogram(_config.Width, _config.Height);
        double x = _random.NextDouble() * (XMAX - XMIN) + XMIN;
        double y = _random.NextDouble() * (YMAX - YMIN) + YMIN;
        var (cr, cg, cb) = (0.0, 0.0, 0.0);

        var totalWeight = _functions.Sum(f => f.Weight);
        var cumulative = _functions.Select(f => f.Weight / totalWeight).ToArray();
        for (int i = 1; i < cumulative.Length; i++)
        {
            cumulative[i] += cumulative[i - 1];
        }

        for (int warmup = 0; warmup < 20; warmup++)
        {
            var func = ChooseFunction(cumulative);
            (x, y) = ApplyFunction(func, x, y, ref cr, ref cg, ref cb);
        }

        for (int i = 0; i < _config.IterationCount; i++)
        {
            var func = ChooseFunction(cumulative);
            (x, y) = ApplyFunction(func, x, y, ref cr, ref cg, ref cb);

            for (int s = 0; s < _config.SymmetryLevel; s++)
            {
                double angle = 2 * Math.PI * s / _config.SymmetryLevel;
                double rx = x * Math.Cos(angle) - y * Math.Sin(angle);
                double ry = x * Math.Sin(angle) + y * Math.Cos(angle);

                if (rx < XMIN || rx > XMAX || ry < YMIN || ry > YMAX)
                {
                    continue;
                }

                int px = (int)((rx - XMIN) / (XMAX - XMIN) * _config.Width);
                int py = (int)((ry - YMIN) / (YMAX - YMIN) * _config.Height);

                if (px < 0 || px >= _config.Width || py < 0 || py >= _config.Height)
                {
                    continue;
                }

                histogram.AddPoint(px, py, cr, cg, cb);
            }
        }

        return histogram;
    }

    private FlameFunction ChooseFunction(double[] cumulative)
    {
        double r = _random.NextDouble();
        for (int i = 0; i < cumulative.Length; i++)
        {
            if (r <= cumulative[i])
            {
                return _functions[i];
            }
        }
        return _functions[^1];
    }

    private (double X, double Y) ApplyFunction(FlameFunction f, double x, double y, ref double cr, ref double cg, ref double cb)
    {
        var (ax, ay) = f.Affine.Apply(x, y);

        var (vx, vy) = (ax, ay);
        if (_variations.TryGetValue(f.Variation, out var variation))
        {
            var result = variation.Apply(ax, ay);
            vx = result.X;
            vy = result.Y;
        }

        (var fr, var fg, var fb) = f.Color;
        cr = (cr + fr) / 2.0;
        cg = (cg + fg) / 2.0;
        cb = (cb + fb) / 2.0;

        return (vx, vy);
    }
}