using Resources.Messages;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;

public sealed class ColorParser : IPromptInputParser<string, Color?>
{
    public bool TryParse(string input, out Color? value, out string? errorMessage, object? additionalContext = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        try
        {
            string trimmedInput = input.Trim();
            object colorObj = ColorConverter.ConvertFromString(trimmedInput);
            if (colorObj is Color color)
            {
                value = color;
                errorMessage = null;
                return true;
            }

            value = null;
            errorMessage = ErrorMessages.InvalidFormat;
            return false;
        }
        catch
        {
            value = null;
            errorMessage = ErrorMessages.InvalidFormat;
            return false;
        }
    }
}
