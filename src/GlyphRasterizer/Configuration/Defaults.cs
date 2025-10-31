using GlyphRasterizer.Lookup.Format.Image;
using System.Collections.Immutable;
using System.Windows.Media;

namespace GlyphRasterizer.Configuration;

public static class Defaults
{
    public const float FontSize = 100f;
    public const int ImageSize = 256;
    public static readonly Color Color = Colors.Black;
    public static readonly ImmutableList<ImageFormat> ImageFormats = [ImageFormat.Png];
    public static readonly PixelFormat PixelFormat = PixelFormats.Pbgra32;
}