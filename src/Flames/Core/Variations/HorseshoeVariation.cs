namespace Flames.Core.Variations;

public class HorseshoeVariation : IVariation
{
    public string Name => "horseshoe";
    public (double X, double Y) Apply(double x, double y)
    {
        double r = Math.Sqrt(x * x + y * y);
        if (r == 0)
        {
            return (0, 0);
        }

        return ((x - y) * (x + y) / r, 2 * x * y / r);
    }
}