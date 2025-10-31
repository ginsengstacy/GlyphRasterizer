using Resources.Messages;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer.BitmapEncoders;

public sealed class IcoBitmapEncoder : IBitmapEncoder
{
    public IList<BitmapFrame> Frames { get; } = [];

    private const int IconDirSize = 6;
    private const int IconEntrySize = 16;
    private const int MinIconDimension = 1;
    private const int MaxIconDimension = 256;

    public void Save(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        EnsureHasFrames(Frames);

        List<byte[]> encodedFrames = EncodeFramesToPngBytes(Frames);

        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

        WriteIconHeader(writer, encodedFrames.Count);
        List<IconDirEntry> entries = CreateDirectoryEntries(Frames, encodedFrames);
        WriteDirectoryEntries(writer, entries);
        WriteImagePayloads(writer, encodedFrames);

        writer.Flush();
    }

    private static void EnsureHasFrames(IList<BitmapFrame> frames)
    {
        if (frames.Count == 0)
        {
            throw new NotSupportedException(ExceptionMessages.NoEncodableFrames);
        }
    }

    private static List<byte[]> EncodeFramesToPngBytes(IList<BitmapFrame> frames) =>
        [.. frames.Where(f => f is not null).Select(EncodeFrameToPng)];

    private static byte[] EncodeFrameToPng(BitmapFrame frame)
    {
        using var ms = new MemoryStream();
        var pngEncoder = new PngBitmapEncoder();
        pngEncoder.Frames.Add(frame);
        pngEncoder.Save(ms);
        return ms.ToArray();
    }

    private static void WriteIconHeader(BinaryWriter writer, int imageCount)
    {
        const ushort Reserved = 0;
        const ushort IconType = 1;

        writer.Write(Reserved);
        writer.Write(IconType);
        writer.Write((ushort)imageCount);
    }

    private static List<IconDirEntry> CreateDirectoryEntries(IList<BitmapFrame> frames, List<byte[]> payloads)
    {
        int headerSize = IconDirSize + IconEntrySize * payloads.Count;
        int offset = headerSize;

        var entries = new List<IconDirEntry>(payloads.Count);
        for (int i = 0; i < payloads.Count; i++)
        {
            BitmapFrame frame = frames[i];
            byte[] payload = payloads[i];

            int width = Math.Clamp(frame.PixelWidth, MinIconDimension, MaxIconDimension);
            int height = Math.Clamp(frame.PixelHeight, MinIconDimension, MaxIconDimension);

            entries.Add(new IconDirEntry
            {
                Width = (byte)(width == 256 ? 0 : width),
                Height = (byte)(height == 256 ? 0 : height),
                ColorCount = 0,
                Reserved = 0,
                Planes = 1,
                BitCount = 32,
                BytesInRes = (uint)payload.Length,
                ImageOffset = (uint)offset
            });

            offset += payload.Length;
        }

        return entries;
    }

    private static void WriteDirectoryEntries(BinaryWriter writer, IEnumerable<IconDirEntry> entries)
    {
        foreach (IconDirEntry entry in entries)
        {
            writer.Write(entry.Width);
            writer.Write(entry.Height);
            writer.Write(entry.ColorCount);
            writer.Write(entry.Reserved);
            writer.Write(entry.Planes);
            writer.Write(entry.BitCount);
            writer.Write(entry.BytesInRes);
            writer.Write(entry.ImageOffset);
        }
    }

    private static void WriteImagePayloads(BinaryWriter writer, IEnumerable<byte[]> payloads)
    {
        foreach (byte[] payload in payloads)
        {
            writer.Write(payload);
        }
    }

    private sealed class IconDirEntry
    {
        public byte Width;
        public byte Height;
        public byte ColorCount;
        public byte Reserved;
        public ushort Planes;
        public ushort BitCount;
        public uint BytesInRes;
        public uint ImageOffset;
    }
}
