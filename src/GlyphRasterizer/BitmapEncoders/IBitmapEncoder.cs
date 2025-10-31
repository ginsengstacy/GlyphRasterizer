using System.IO;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer.BitmapEncoders;

public interface IBitmapEncoder
{
    void Save(Stream stream);
    IList<BitmapFrame> Frames { get; }
}
