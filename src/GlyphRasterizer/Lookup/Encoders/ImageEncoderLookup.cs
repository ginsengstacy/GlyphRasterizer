using GlyphRasterizer.BitmapEncoders;
using GlyphRasterizer.Lookup.Format.Image;
using System.Collections.Immutable;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer.Lookup.Encoders;

public static class ImageEncoderLookup
{
    public static readonly ImmutableDictionary<ImageFormat, Func<IBitmapEncoder>> Lookup =
        ImmutableDictionary.Create<ImageFormat, Func<IBitmapEncoder>>()
            .Add(ImageFormat.Png, () => new WpfBitmapEncoderAdapter(new PngBitmapEncoder()))
            .Add(ImageFormat.Jpeg, () => new WpfBitmapEncoderAdapter(new JpegBitmapEncoder()))
            .Add(ImageFormat.Bmp, () => new WpfBitmapEncoderAdapter(new BmpBitmapEncoder()))
            .Add(ImageFormat.Tiff, () => new WpfBitmapEncoderAdapter(new TiffBitmapEncoder()))
            .Add(ImageFormat.Ico, () => new IcoBitmapEncoder());
}
