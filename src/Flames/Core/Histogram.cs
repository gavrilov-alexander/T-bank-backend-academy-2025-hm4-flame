namespace Flames.Core;

public class Histogram
{
    public readonly int Width;
    public readonly int Height;
    public readonly double[] R;
    public readonly double[] G;
    public readonly double[] B;
    public readonly int[] Counts;

    public Histogram(int width, int height)
    {
        Width = width;
        Height = height;
        int size = width * height;
        R = new double[size];
        G = new double[size];
        B = new double[size];
        Counts = new int[size];
    }

    public void AddPoint(int x, int y, double r, double g, double b)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return;
        }

        int idx = y * Width + x;
        R[idx] += r;
        G[idx] += g;
        B[idx] += b;
        Counts[idx]++;
    }

    public void Merge(Histogram other)
    {
        for (int i = 0; i < R.Length; i++)
        {
            R[i] += other.R[i];
            G[i] += other.G[i];
            B[i] += other.B[i];
            Counts[i] += other.Counts[i];
        }
    }

    public int MaxCount => Counts.Length > 0 ? Counts.Max() : 1;
}