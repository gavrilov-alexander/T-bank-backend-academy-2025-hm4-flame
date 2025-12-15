namespace Flames.Core.Models;

public class AffineParams
{
    public double A { get; set; } = 0.5;
    public double B { get; set; } = 0.0;
    public double C { get; set; } = 0.0;
    public double D { get; set; } = 0.0;
    public double E { get; set; } = 0.5;
    public double F { get; set; } = 0.0;

    public (double X, double Y) Apply(double x, double y)
    {
        return (A * x + B * y + C, D * x + E * y + F);
    }
}