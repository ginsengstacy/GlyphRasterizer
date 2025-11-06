using GlyphRasterizer.Configuration;
using ImageMagick;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;

public sealed class ImageFormatParser : IPromptInputParser<string, ImmutableArray<MagickFormat>?>
{
    public static readonly ImmutableDictionary<string, MagickFormat> _availableImageFormatNamesToValuesMap =
        AppConfig.AvailableImageFormats
            .ToImmutableDictionary(
                f => Enum.GetName(f)!,
                f => f,
                StringComparer.OrdinalIgnoreCase
            );

    public bool TryParse(string input, out ImmutableArray<MagickFormat>? value, out string? errorMessage, object? additionalContext = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        string[] tokens = [.. input.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim())];
        if (tokens.Length == 0)
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        var parsedFormats = new HashSet<MagickFormat>();
        var invalidEntries = new List<string>();

        foreach (string token in tokens)
        {
            if (_availableImageFormatNamesToValuesMap.TryGetValue(token, out MagickFormat format))
            {
                parsedFormats.Add(format);
            }
            else
            {
                invalidEntries.Add(token);
            }
        }

        if (invalidEntries.Count > 0)
        {
            value = null;
            errorMessage = string.Format(ErrorMessages.InvalidFormats_FormatString, string.Join(", ", invalidEntries.Select(e => $"'{e}'")));
            return false;
        }

        value = [.. parsedFormats];
        errorMessage = null;
        return true;
    }
}
