using System.Collections.Immutable;

namespace GlyphRasterizer.Lookup.Format.Font;

public static class FontFormatDataLookup
{
    public static readonly ImmutableDictionary<FontFormat, FormatLookupData> Lookup =
        ImmutableDictionary.Create<FontFormat, FormatLookupData>()
            .Add(FontFormat.Ttf, new FormatLookupData(".ttf", ImmutableHashSet.Create("ttf", ".ttf")))
            .Add(FontFormat.Otf, new FormatLookupData(".otf", ImmutableHashSet.Create("otf", ".otf"))
        );
}
