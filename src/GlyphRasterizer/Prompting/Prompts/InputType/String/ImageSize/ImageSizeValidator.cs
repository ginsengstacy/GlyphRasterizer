using GlyphRasterizer.Configuration;
using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;

public sealed class ImageSizeValidator : IPromptValueValidator<int?>
{
    public bool IsValid(int? input, out string? errorMessage)
    {
        if (input == null)
        {
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        if (input < Config.MinImageSize)
        {
            errorMessage = string.Format(ErrorMessages.SizeTooSmall_FormatString, Config.MinImageSize);
            return false;
        }

        if (input > Config.MaxImageSize)
        {
            errorMessage = string.Format(ErrorMessages.SizeTooLarge_FormatString, Config.MaxImageSize);
            return false;
        }

        errorMessage = null;
        return true;
    }
}
