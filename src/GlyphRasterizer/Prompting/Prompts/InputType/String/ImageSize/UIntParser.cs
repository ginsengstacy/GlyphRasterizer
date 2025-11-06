using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;

public sealed class UIntParser : IPromptInputParser<string, uint?>
{
    public bool TryParse(string input, out uint? value, out string? errorMessage, object? additionalContext = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        if (!uint.TryParse(input, out var size))
        {
            value = null;
            errorMessage = ErrorMessages.InvalidFormat;
            return false;
        }

        value = size;
        errorMessage = null;
        return true;
    }
}
