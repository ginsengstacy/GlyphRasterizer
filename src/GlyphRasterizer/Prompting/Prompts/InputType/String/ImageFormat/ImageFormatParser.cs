using GlyphRasterizer.Lookup;
using GlyphRasterizer.Lookup.Format.Image;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;

public sealed class ImageFormatParser : IPromptInputParser<string, ImmutableList<Lookup.Format.Image.ImageFormat>?>
{
    private static readonly HashSet<Lookup.Format.Image.ImageFormat> Formats = [.. Enum.GetValues<Lookup.Format.Image.ImageFormat>()];

    public bool TryParse(string input, out ImmutableList<Lookup.Format.Image.ImageFormat>? value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        string trimmedInput = input.Trim();

        if (trimmedInput.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            value = [.. Formats];
            errorMessage = null;
            return true;
        }

        string[] tokens = [.. trimmedInput
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.Trim()
        )];

        if (tokens.Length == 0)
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        var parsedFormats = new HashSet<Lookup.Format.Image.ImageFormat>();
        var invalidEntries = new List<string>();

        foreach (string token in tokens)
        {
            bool isValidImageFormatInput = LookupHelpers.TryGetKeyFromRepresentation(
                ImageFormatDataLookup.Lookup,
                token,
                out Lookup.Format.Image.ImageFormat format,
                StringComparer.OrdinalIgnoreCase
            );

            if (isValidImageFormatInput)
            {
                parsedFormats.Add(format);
                continue;
            }

            invalidEntries.Add(token);
        }

        if (invalidEntries.Count > 0)
        {
            value = null;
            errorMessage = string.Format(
                ErrorMessages.InvalidFormats_FormatString,
                string.Join(", ", invalidEntries.Select(e => $"'{e}'"))
            );

            return false;
        }

        value = [.. parsedFormats];
        errorMessage = null;
        return true;
    }
}
