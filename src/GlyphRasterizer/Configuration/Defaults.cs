using ImageMagick;
using System.Collections.Immutable;
using System.Windows.Media;

namespace GlyphRasterizer.Configuration;

public static class Defaults
{
    public static readonly Color Color = Colors.Black;
    public static readonly ImmutableArray<MagickFormat> ImageFormats = [MagickFormat.Png];
}