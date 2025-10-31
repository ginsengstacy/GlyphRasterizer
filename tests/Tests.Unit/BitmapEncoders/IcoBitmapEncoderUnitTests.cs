using FluentAssertions;
using GlyphRasterizer.BitmapEncoders;
using System.IO;

namespace Tests.Unit.BitmapEncoders;

public class IcoBitmapEncoderUnitTests
{
    [Fact]
    public void Save_Should_ThrowNotSupported_When_NoFrames()
    {
        var encoder = new IcoBitmapEncoder();
        using var stream = new MemoryStream();

        FluentActions.Invoking(() => encoder.Save(stream))
            .Should().Throw<NotSupportedException>();
    }
}
