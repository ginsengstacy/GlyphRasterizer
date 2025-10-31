using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;

public sealed class ImageSizePrompt(IntParser intParser, ImageSizeValidator imageSizeValidator, PromptActionParser promptActionParser)
    : PromptBase<string, int?>(promptActionParser)
{
    protected override string Message =>
        string.Format(
            PromptMessages.Size_FormatString,
            Config.MinImageSize,
            Config.MaxImageSize
        );

    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, int?> Parser => intParser;
    protected override IPromptValueValidator<int?> Validator { get; } = imageSizeValidator;
    protected override Action<SessionContext, int?> ValueUpdater { get; } = (context, value) => context.ImageSize = value;
}