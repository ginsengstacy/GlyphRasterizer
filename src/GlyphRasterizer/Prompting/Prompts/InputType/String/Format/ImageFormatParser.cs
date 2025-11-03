using GlyphRasterizer.Lookup;
using GlyphRasterizer.Lookup.Format.Image;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Format;

public sealed class ImageFormatParser : IPromptInputParser<string, ImmutableArray<ImageFormat>?>
{
    private static readonly HashSet<ImageFormat> Formats = [.. Enum.GetValues<ImageFormat>()];

    public bool TryParse(string input, out ImmutableArray<ImageFormat>? value, out string? errorMessage)
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

        foreach (string token in tokens)
        {
            bool isValidImageFormatInput = LookupHelpers.TryGetKeyFromRepresentation(
                ImageFormatDataLookup.Lookup,
                token,
                out ImageFormat format,
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
