using System.Text.RegularExpressions;

namespace GlyphRasterizer.Extensions;

public static partial class StringExtensions
{
    [GeneratedRegex(@"\{.*?\}")]
    private static partial Regex PlaceholderSplitRegex();

    public static bool MatchesFormat(this string actual, string format)
    {
        string[] parts = [.. PlaceholderSplitRegex().Split(format).Where(p => p.Length > 0)];

        int index = 0;
        foreach (string part in parts)
        {
            index = actual.IndexOf(part, index, StringComparison.Ordinal);
            if (index < 0)
            {
                return false;
            }

            index += part.Length;
        }

        return true;
    }
}
