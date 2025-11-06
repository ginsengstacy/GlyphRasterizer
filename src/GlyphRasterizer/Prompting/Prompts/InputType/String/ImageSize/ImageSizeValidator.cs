using GlyphRasterizer.Configuration;
using ImageMagick;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;

public sealed class ImageSizeValidator : IPromptValueValidator<uint?>
{
    public bool IsValid(uint? input, out string? errorMessage, object? additionalContext = null)
    {
        if (input is null)
        {
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        if (input < AppConfig.MinImageSize)
        {
            errorMessage = string.Format(ErrorMessages.SizeTooSmall_FormatString, AppConfig.MinImageSize);
            return false;
        }

        if (input > AppConfig.MaxImageSize)
        {
            errorMessage = string.Format(ErrorMessages.SizeTooLarge_FormatString, AppConfig.MaxImageSize);
            return false;
        }

        var selectedFormats = (ImmutableArray<MagickFormat>)additionalContext!;
        if (selectedFormats.Contains(MagickFormat.Ico) && input > AppConfig.MaxIcoSize)
        {
            errorMessage = string.Format(ErrorMessages.SizeTooLargeForIco_FormatString, AppConfig.MaxIcoSize);
            return false;
        }

        errorMessage = null;
        return true;
    }
}
