namespace Flames.Core;

public static class ToneMapper
{
    public static byte[] ApplyToneMapping(Histogram histogram, bool gammaCorrection, double gamma)
    {
        int total = histogram.Width * histogram.Height;
        int maxCount = histogram.MaxCount;
        if (maxCount == 0)
        {
            maxCount = 1;
        }

        var result = new byte[total * 3];

        for (int i = 0; i < total; i++)
        {
            if (histogram.Counts[i] == 0)
            {
                result[i * 3] = 0;
                result[i * 3 + 1] = 0;
                result[i * 3 + 2] = 0;
                continue;
            }

            double density = Math.Log10(histogram.Counts[i] + 1) / Math.Log10(maxCount + 1);

            if (gammaCorrection)
            {
                density = Math.Pow(density, 1.0 / gamma);
            }

            double r = histogram.R[i] / histogram.Counts[i];
            double g = histogram.G[i] / histogram.Counts[i];
            double b = histogram.B[i] / histogram.Counts[i];

            result[i * 3] = (byte)Math.Clamp(r * density * 255, 0, 255);
            result[i * 3 + 1] = (byte)Math.Clamp(g * density * 255, 0, 255);
            result[i * 3 + 2] = (byte)Math.Clamp(b * density * 255, 0, 255);
        }

        return result;
    }
}