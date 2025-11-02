using Resources.Messages;
using System.Text;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;

public readonly record struct Glyph
{
    public string UnicodeValue { get; init; }
    public string UnicodeLabel { get; init; }
    public int CodePoint { get; init; }

    public Glyph(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException(ErrorMessages.InvalidFormat);
        }

        string normalized = input.Normalize(NormalizationForm.FormC);

        UnicodeValue = normalized;

        Rune firstRune = normalized.EnumerateRunes().First();
        CodePoint = firstRune.Value;

        UnicodeLabel = string.Join(" ", normalized.EnumerateRunes().Select(r => $"U+{r.Value:X4}"));
    }

    public override string ToString() => UnicodeLabel;
}
