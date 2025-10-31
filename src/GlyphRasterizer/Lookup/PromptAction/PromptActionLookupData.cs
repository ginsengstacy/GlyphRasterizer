using System.Collections.Immutable;

namespace GlyphRasterizer.Lookup.PromptAction;

public readonly struct PromptActionLookupData(string displayName, ImmutableHashSet<string> representations) : ILookupableData
{
    public string DisplayName { get; } = displayName;
    public ImmutableHashSet<string> Representations { get; } = representations;
}