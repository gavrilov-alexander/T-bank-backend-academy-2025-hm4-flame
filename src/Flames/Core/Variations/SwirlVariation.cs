namespace Flames.Core.Variations;

public class SwirlVariation : IVariation
{
    public string Name => "swirl";
    public (double X, double Y) Apply(double x, double y)
    {
        double r2 = x * x + y * y;
        double sinr2 = Math.Sin(r2);
        double cosr2 = Math.Cos(r2);
        return (x * sinr2 - y * cosr2, x * cosr2 + y * sinr2);
    }
}