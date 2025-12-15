namespace Flames.Core.Variations;

public class LinearVariation : IVariation
{
    public string Name => "linear";
    public (double X, double Y) Apply(double x, double y) => (x, y);
}