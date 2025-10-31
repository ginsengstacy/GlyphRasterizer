using GlyphRasterizer.Lookup.Encoders;
using GlyphRasterizer.Lookup.Format.Image;
using System.IO;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer.BitmapEncoders;

public sealed class BitmapEncoderService
{
    public byte[] Encode(RenderTargetBitmap bitmap, ImageFormat imageFormat)
    {
        IBitmapEncoder encoder = ImageEncoderLookup.Lookup[imageFormat]();
        BitmapFrame frame = BitmapFrame.Create(bitmap);
        frame.Freeze();
        encoder.Frames.Add(frame);

        using var memoryStream = new MemoryStream();
        encoder.Save(memoryStream);
        return memoryStream.ToArray();
    }
}
