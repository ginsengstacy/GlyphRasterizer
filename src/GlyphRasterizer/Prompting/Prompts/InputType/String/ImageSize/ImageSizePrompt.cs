using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Terminal;
using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;

public sealed class ImageSizePrompt(UIntParser uintParser, ImageSizeValidator imageSizeValidator, CommandTypeParser commandTypeParser)
    : PromptBase<string, uint?>(commandTypeParser)
{
    protected override string Message => string.Format(PromptMessages.Size_FormatString, AppConfig.MinImageSize, AppConfig.MaxImageSize);

    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, uint?> Parser => uintParser;
    protected override IPromptValueValidator<uint?> Validator { get; } = imageSizeValidator;
    protected override Action<SessionContext, uint?> ValueUpdater { get; } = (context, value) => context.ImageSize = value;
}