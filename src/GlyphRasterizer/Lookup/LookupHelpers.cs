namespace GlyphRasterizer.Lookup;

public static class LookupHelpers
{
    public static bool TryGetKeyFromRepresentation<TKey, TData>(
        IReadOnlyDictionary<TKey, TData> lookup,
        string representation,
        out TKey key,
        StringComparer? stringComparer = null
    )
        where TKey : Enum
        where TData : ILookupableData
    {
        foreach (KeyValuePair<TKey, TData> kvp in lookup)
        {
            if (kvp.Value.Representations.Contains(representation, stringComparer))
            {
                key = kvp.Key;
                return true;
            }
        }

        key = default!;
        return false;
    }
}