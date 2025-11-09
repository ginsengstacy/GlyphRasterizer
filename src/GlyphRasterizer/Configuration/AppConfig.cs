using ImageMagick;
using System.Collections.Immutable;
using System.Windows.Media;

namespace GlyphRasterizer.Configuration;

public static class AppConfig
{
    public static readonly PixelFormat PixelFormat = PixelFormats.Pbgra32;
    public const int MaxImageSize = 2048;
    public const int Padding = 1;
    public const int Dpi = 96;

    public static readonly ImmutableArray<MagickFormat> AvailableImageFormats =
        ImmutableArray.Create(
            MagickFormat.Png,
            MagickFormat.Jpeg,
            MagickFormat.WebP,
            MagickFormat.Tiff,
            MagickFormat.Ico,
            MagickFormat.Avif,
            MagickFormat.Bmp,
            MagickFormat.Psd,
            MagickFormat.Pcx,
            MagickFormat.Tga,
            MagickFormat.Pnm
        );
}