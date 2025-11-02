using GlyphRasterizer.Configuration;
using GlyphRasterizer.Lookup.Format.Image;
using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;

public sealed class ImageFormatPrompt(ImageFormatParser imageFormatParser, PromptActionParser promptActionParser)
    : PromptBase<string, ImmutableList<Lookup.Format.Image.ImageFormat>?>(promptActionParser)
{
    private static readonly string[] _imageFormatNames = Enum.GetNames<Lookup.Format.Image.ImageFormat>();

    protected override string Message =>
        string.Format(
            PromptMessages.ImageFormat_FormatString,
            _imageFormatNames.ElementAtOrDefault(0),
            _imageFormatNames.ElementAtOrDefault(1),
            string.Join(", ", _imageFormatNames),
            Environment.NewLine
        );

    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, ImmutableList<Lookup.Format.Image.ImageFormat>?> Parser { get; } = imageFormatParser;
    protected override Action<SessionContext, ImmutableList<Lookup.Format.Image.ImageFormat>?> ValueUpdater { get; } =
        (context, value) => context.ImageFormats = value;
}