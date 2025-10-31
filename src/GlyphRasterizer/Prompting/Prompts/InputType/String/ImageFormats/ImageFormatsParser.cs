using GlyphRasterizer.Lookup;
using GlyphRasterizer.Lookup.Format.Image;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormats;

public sealed class ImageFormatsParser : IPromptInputParser<string, ImmutableList<ImageFormat>?>
{
    private static readonly HashSet<ImageFormat> Formats = [.. Enum.GetValues<ImageFormat>()];

    public bool TryParse(string input, out ImmutableList<ImageFormat>? value, out string? errorMessage)
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

        var parsedFormats = new HashSet<ImageFormat>();
        var invalidEntries = new List<string>();

        foreach (var entry in tokens)
        {
            var isValidImageFormatInput = LookupHelpers.TryGetKeyFromRepresentation(
                ImageFormatDataLookup.Lookup,
                entry,
                out var format,
                StringComparer.OrdinalIgnoreCase
            );

            if (isValidImageFormatInput)
            {
                parsedFormats.Add(format);
                continue;
            }

            invalidEntries.Add(entry);
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
