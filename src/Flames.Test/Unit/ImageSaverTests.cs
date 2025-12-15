using Flames.Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flames.Test.Unit;
public class ImageSaverTests
{
    [Fact]
    public async Task ImageSaver_SavePngAsync_CreatesValidPng()
    {
        var pixels = new byte[100 * 100 * 3];
        await ImageSaver.SavePngAsync(pixels, 100, 100, "test.png");

        using var image = SixLabors.ImageSharp.Image.Load("test.png");
        image.Width.Should().Be(100);
        image.Height.Should().Be(100);

        File.Delete("test.png");
    }
}
