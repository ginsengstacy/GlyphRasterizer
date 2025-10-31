using System.Collections.Immutable;

namespace GlyphRasterizer.Lookup.Format.Image;

public static class ImageFormatDataLookup
{
    public static readonly ImmutableDictionary<ImageFormat, FormatLookupData> Lookup =
        ImmutableDictionary.Create<ImageFormat, FormatLookupData>()
            .Add(ImageFormat.Png, new FormatLookupData(".png", ImmutableHashSet.Create("png", ".png")))
            .Add(ImageFormat.Jpeg, new FormatLookupData(".jpeg", ImmutableHashSet.Create("jpeg", ".jpeg", "jpg", ".jpg")))
            .Add(ImageFormat.Bmp, new FormatLookupData(".bmp", ImmutableHashSet.Create("bmp", ".bmp")))
            .Add(ImageFormat.Tiff, new FormatLookupData(".tiff", ImmutableHashSet.Create("tiff", ".tiff")))
            .Add(ImageFormat.Ico, new FormatLookupData(".ico", ImmutableHashSet.Create("ico", ".ico"))
        );
}
