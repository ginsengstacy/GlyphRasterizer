using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Terminal;
using ImageMagick;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;

public sealed class ImageFormatPrompt(ImageFormatParser imageFormatParser, CommandTypeParser commandTypeParser)
    : PromptBase<string, ImmutableArray<MagickFormat>?>(commandTypeParser)
{
    private static readonly string[] _imageFormatNames = [.. AppConfig.AvailableImageFormats.Select(f => Enum.GetName(f)!)];

    protected override string Message =>
        string.Format(
            PromptMessages.Format_FormatString,
            _imageFormatNames[0],
            _imageFormatNames[1],
            Environment.NewLine,
            string.Join(" | ", _imageFormatNames),
            Environment.NewLine
        );

    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, ImmutableArray<MagickFormat>?> Parser { get; } = imageFormatParser;
    protected override Action<SessionContext, ImmutableArray<MagickFormat>?> ValueUpdater { get; } = (context, value) => context.ImageFormats = value;
}