using GlyphRasterizer.Configuration;
using GlyphRasterizer.Lookup.Format.Image;
using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Format;

public sealed class ImageFormatPrompt(ImageFormatParser formatParser, PromptActionParser promptActionParser)
    : PromptBase<string, ImmutableArray<ImageFormat>?>(promptActionParser)
{
    private static readonly string[] _imageFormatNames = Enum.GetNames<ImageFormat>();

    protected override string Message =>
        string.Format(
            PromptMessages.ImageFormat_FormatString,
            _imageFormatNames.ElementAtOrDefault(0),
            _imageFormatNames.ElementAtOrDefault(1),
            string.Join(", ", _imageFormatNames),
            Environment.NewLine
        );

    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, ImmutableArray<ImageFormat>?> Parser { get; } = formatParser;
    protected override Action<SessionContext, ImmutableArray<ImageFormat>?> ValueUpdater { get; } =
        (context, value) => context.Formats = value;
}