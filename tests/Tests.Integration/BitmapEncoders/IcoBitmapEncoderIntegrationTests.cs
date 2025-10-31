using FluentAssertions;
using GlyphRasterizer.BitmapEncoders;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tests.Integration.BitmapEncoders;

public class IcoBitmapEncoderIntegrationTests
{
    [Fact]
    public void Save_Should_CreateValidIcoFile_When_SingleFrame()
    {
        var encoder = new IcoBitmapEncoder();

        var frame = BitmapFrame.Create(BitmapSource.Create(
            1, 1, 96, 96,
            PixelFormats.Bgra32,
            null,
            new byte[4],
            4));
        encoder.Frames.Add(frame);

        using var stream = new MemoryStream();

        encoder.Save(stream);
        stream.Position = 0;

        var bytes = stream.ToArray();
        bytes.Length.Should().BeGreaterThan(22); // header + one entry + payload
        bytes[0..2].Should().Equal(0x00, 0x00);  // Reserved
        bytes[2..4].Should().Equal(0x01, 0x00);  // Type = Icon
        bytes[4..6].Should().Equal(0x01, 0x00);  // Image count = 1

        stream.Position = 0;
        var decoder = BitmapDecoder.Create(
            stream,
            BitmapCreateOptions.None,
            BitmapCacheOption.OnLoad);

        decoder.Frames.Should().HaveCount(1);
        decoder.Frames.First().PixelWidth.Should().Be(1);
        decoder.Frames.First().PixelHeight.Should().Be(1);
    }
}
