using System.IO;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer.BitmapEncoders;

public sealed class WpfBitmapEncoderAdapter(BitmapEncoder encoder) : IBitmapEncoder
{
    private readonly BitmapEncoder _encoder = encoder ?? throw new ArgumentNullException(nameof(encoder));
    public IList<BitmapFrame> Frames => _encoder.Frames;
    public void Save(Stream stream) => _encoder.Save(stream);
}
