using System.Collections.Immutable;

namespace GlyphRasterizer.Lookup;

public interface ILookupableData
{
    ImmutableHashSet<string> Representations { get; }
}