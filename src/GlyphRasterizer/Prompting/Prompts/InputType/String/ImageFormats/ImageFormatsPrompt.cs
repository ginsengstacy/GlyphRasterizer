using GlyphRasterizer.Configuration;
using GlyphRasterizer.Lookup.Format.Image;
using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormats;

public sealed class ImageFormatsPrompt(ImageFormatsParser imageFormatsParser, PromptActionParser promptActionParser)
    : PromptBase<string, ImmutableList<ImageFormat>?>(promptActionParser)
{
    private static readonly string[] _imageFormatNames = Enum.GetNames<ImageFormat>();

    protected override string Message =>
        string.Format(
            PromptMessages.ImageFormats_FormatString,
            _imageFormatNames.ElementAtOrDefault(0),
            _imageFormatNames.ElementAtOrDefault(1),
            string.Join(", ", _imageFormatNames),
            Environment.NewLine
        );

    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, ImmutableList<ImageFormat>?> Parser { get; } = imageFormatsParser;
    protected override Action<SessionContext, ImmutableList<ImageFormat>?> ValueUpdater { get; } =
        (context, value) => context.ImageFormats = value;
}