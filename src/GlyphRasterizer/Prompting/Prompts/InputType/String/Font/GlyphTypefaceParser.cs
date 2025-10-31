using GlyphRasterizer.Lookup.Format.Font;
using Resources.Messages;
using System.IO;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Font;

public sealed class GlyphTypefaceParser : IPromptInputParser<string, GlyphTypeface?>
{
    public bool TryParse(string input, out GlyphTypeface? value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        string trimmedInput = input.Trim('"');
        string typefacePath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(trimmedInput));
        string extension = Path.GetExtension(typefacePath);

        var hasMatchingExtension = FontFormatDataLookup.Lookup.Any(f => f.Value.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));
        if (!hasMatchingExtension)
        {
            value = null;
            errorMessage = ErrorMessages.InvalidFileExtension;
            return false;
        }

        if (!File.Exists(typefacePath))
        {
            value = null;
            errorMessage = ErrorMessages.PathNotFound;
            return false;
        }

        return TryLoadGlyphTypeface(typefacePath, out value, out errorMessage);
    }

    private static bool TryLoadGlyphTypeface(string fontPath, out GlyphTypeface? value, out string? errorMessage)
    {
        try
        {
            value = new GlyphTypeface(new Uri(fontPath, UriKind.Absolute));
            errorMessage = null;
            return true;
        }
        catch (Exception ex) when (ex is ArgumentException
                                   or FileFormatException
                                   or IOException
                                   or UnauthorizedAccessException)
        {
            value = null;
            errorMessage = string.Format(ExceptionMessages.FailedToLoad_FormatString, fontPath, ex.Message);
        }

        return false;
    }
}
