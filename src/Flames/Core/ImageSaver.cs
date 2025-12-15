using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Flames.Core;

public static class ImageSaver
{
    public static async Task SavePngAsync(byte[] pixelData, int width, int height, string outputPath)
    {
        var image = Image.LoadPixelData<Rgb24>(pixelData, width, height);
        await image.SaveAsPngAsync(outputPath);
    }
}