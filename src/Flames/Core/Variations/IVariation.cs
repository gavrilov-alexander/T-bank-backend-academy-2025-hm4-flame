namespace Flames.Core.Variations;

public interface IVariation
{
    string Name { get; }
    (double X, double Y) Apply(double x, double y);
}