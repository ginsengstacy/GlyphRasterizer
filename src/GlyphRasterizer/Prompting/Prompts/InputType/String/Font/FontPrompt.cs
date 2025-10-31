using GlyphRasterizer.Configuration;
using GlyphRasterizer.Lookup.Format.Font;
using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Font;

public sealed class FontPrompt(GlyphTypefaceParser typefaceParser, PromptActionParser promptActionParser)
    : PromptBase<string, GlyphTypeface?>(promptActionParser)
{
    protected override string Message =>
        string.Format(
            PromptMessages.FontPath_FormatString,
            string.Join(", ", Enum.GetNames<FontFormat>()),
            Environment.NewLine
        );

    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, GlyphTypeface?> Parser { get; } = typefaceParser;
    protected override Action<SessionContext, GlyphTypeface?> ValueUpdater { get; } = (context, value) => context.GlyphTypeface = value;
}