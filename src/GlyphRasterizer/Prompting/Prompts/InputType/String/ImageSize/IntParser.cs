using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;

public sealed class IntParser : IPromptInputParser<string, int?>
{
    public bool TryParse(string input, out int? value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        if (!int.TryParse(input, out var size))
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
