using Resources.Messages;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;

public sealed class GlyphsParser : IPromptInputParser<GlyphParseContext, ImmutableList<Glyph>?>
{
    public bool TryParse(GlyphParseContext glyphParseContext, out ImmutableList<Glyph>? value, out string? errorMessage)
    {
        string input = glyphParseContext.Input;

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

        GlyphTypeface typeface = glyphParseContext.GlyphTypeface!;
        var parsedGlyphs = new List<Glyph>();
        var uncontainedGlyphs = new List<string>();

        foreach (string token in tokens)
        {
            var glyph = new Glyph(token);
            if (typeface.CharacterToGlyphMap.TryGetValue(glyph.CodePoint, out _))
            {
                parsedGlyphs.Add(glyph);
            }
            else
            {
                uncontainedGlyphs.Add(token);
            }
        }

        if (uncontainedGlyphs.Count > 0)
        {
            value = null;
            string fontName = typeface.FamilyNames.Values.FirstOrDefault() ?? SentinelMessages.UnknownFontName;
            errorMessage = string.Format(
                ErrorMessages.UncontainedGlyphs_FormatString,
                fontName,
                string.Join(", ", uncontainedGlyphs.Select(e => $"'{e}'"))
            );

            return false;
        }

        value = [.. parsedGlyphs];
        errorMessage = null;
        return true;
    }
}
