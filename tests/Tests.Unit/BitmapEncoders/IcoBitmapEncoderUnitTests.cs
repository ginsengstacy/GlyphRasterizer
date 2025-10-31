using System.IO;
using FluentAssertions;
using GlyphRasterizer.BitmapEncoders;

namespace Tests.Unit.BitmapEncoders;

public class IcoBitmapEncoderUnitTests
{
    [Fact]
    public void Save_Should_ThrowNotSupported_When_NoFrames()
    {
        var encoder = new IcoBitmapEncoder();
        using var stream = new MemoryStream();
        Action action = () => encoder.Save(stream);
        action.Should().Throw<NotSupportedException>();
    }
}
