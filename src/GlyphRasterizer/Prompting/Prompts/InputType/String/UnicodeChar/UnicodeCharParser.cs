using Resources.Messages;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.UnicodeChar;

public sealed class UnicodeCharParser : IPromptInputParser<string, ImmutableArray<UnicodeChar>?>
{
    public bool TryParse(string input, out ImmutableArray<UnicodeChar>? value, out string? errorMessage, object? additionalContext = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        string normalizedInput = input.Normalize(NormalizationForm.FormC);
        var tokens = new HashSet<string>();

        TextElementEnumerator? enumerator = StringInfo.GetTextElementEnumerator(normalizedInput);
        while (enumerator.MoveNext())
        {
            var token = (string)enumerator.Current;
            if (!string.IsNullOrWhiteSpace(token))
            {
                tokens.Add(token);
            }
        }

        GlyphTypeface typeface = (GlyphTypeface)additionalContext!;
        var parsedGlyphs = new List<UnicodeChar>();
        var uncontainedGlyphNames = new List<string>();

        foreach (string token in tokens)
        {
            var unicodeChar = new UnicodeChar(token);
            if (typeface.CharacterToGlyphMap.TryGetValue(unicodeChar.CodePoint, out _))
            {
                parsedGlyphs.Add(unicodeChar);
            }
            else
            {
                uncontainedGlyphNames.Add(token);
            }
        }

        if (uncontainedGlyphNames.Count > 0)
        {
            value = null;
            string fontName = typeface.FamilyNames.Values.First() ?? SentinelStrings.UnknownFontName;
            errorMessage = string.Format(
                ErrorMessages.UncontainedGlyphs_FormatString,
                fontName,
                string.Join(", ", uncontainedGlyphNames.Select(e => $"'{e}'"))
            );

            return false;
        }

        value = [.. parsedGlyphs];
        errorMessage = null;
        return true;
    }
}
