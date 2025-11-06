using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Terminal;
using Resources.Messages;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Font;

public sealed class FontPrompt(TypefaceParser typeFaceParser, CommandTypeParser commandTypeParser)
    : PromptBase<string, GlyphTypeface?>(commandTypeParser)
{
    protected override string Message => PromptMessages.Font;

    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, GlyphTypeface?> Parser { get; } = typeFaceParser;
    protected override Action<SessionContext, GlyphTypeface?> ValueUpdater { get; } = (context, value) => context.Typeface = value;
}