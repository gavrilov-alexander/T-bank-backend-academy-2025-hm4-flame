namespace Flames.Core.Models;

public class FlameFunction
{
    public AffineParams Affine { get; set; } = new();
    public string Variation { get; set; } = "linear";
    public double Weight { get; set; } = 1.0;
    public (double R, double G, double B) Color { get; set; } = (0.0, 0.0, 0.0);
}