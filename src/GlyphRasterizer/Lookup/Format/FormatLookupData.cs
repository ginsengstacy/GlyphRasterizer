using System.Collections.Immutable;

namespace GlyphRasterizer.Lookup.Format;

public readonly struct FormatLookupData(string extension, ImmutableHashSet<string> representations) : ILookupableData
{
    public string Extension { get; } = extension;
    public ImmutableHashSet<string> Representations { get; } = representations;
}