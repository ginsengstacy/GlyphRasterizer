using Resources.Messages;
using System.Text;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.UnicodeChar;

public readonly record struct UnicodeChar
{
    public string Value { get; init; }
    public string Label { get; init; }
    public int CodePoint { get; init; }

    public UnicodeChar(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException(ErrorMessages.InvalidFormat);
        }

        string normalized = input.Normalize(NormalizationForm.FormC);

        Value = normalized;

        Rune firstRune = normalized.EnumerateRunes().First();
        CodePoint = firstRune.Value;

        Label = string.Join(" ", normalized.EnumerateRunes().Select(r => $"U+{r.Value:X4}"));
    }

    public override string ToString() => Label;
}