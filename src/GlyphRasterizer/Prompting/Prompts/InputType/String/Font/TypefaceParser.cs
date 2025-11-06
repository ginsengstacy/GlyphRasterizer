using Resources.Messages;
using System.IO;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Font;

public sealed class TypefaceParser : IPromptInputParser<string, GlyphTypeface?>
{
    public bool TryParse(string input, out GlyphTypeface? value, out string? errorMessage, object? additionalContext = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        string trimmedInput = input.Trim('"');
        string fontPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(trimmedInput));

        if (!File.Exists(fontPath))
        {
            value = null;
            errorMessage = ErrorMessages.PathNotFound;
            return false;
        }

        return TryLoadGlyphTypeface(fontPath, out value, out errorMessage);
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
                                      or PathTooLongException
                                      or UnauthorizedAccessException
        )
        {
            errorMessage = string.Format(ExceptionMessages.FailedToLoad_FormatString, fontPath, ex.Message);
            value = null;
            return false;
        }
    }
}
