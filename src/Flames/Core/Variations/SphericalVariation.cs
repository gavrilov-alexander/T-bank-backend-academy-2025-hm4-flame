namespace Flames.Core.Variations;

public class SphericalVariation : IVariation
{
    public string Name => "spherical";
    public (double X, double Y) Apply(double x, double y)
    {
        double r2 = x * x + y * y;
        if (r2 == 0)
        {
            return (0, 0);
        }

        double factor = 1.0 / r2;
        return (factor * x, factor * y);
    }
}