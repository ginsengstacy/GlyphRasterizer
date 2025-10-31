using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;

public sealed class GlyphColorPrompt(GlyphColorParser glyphColorParser, PromptActionParser promptActionParser)
    : PromptBase<string, Color?>(promptActionParser)
{
    protected override string Message => PromptMessages.Color;
    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, Color?> Parser { get; } = glyphColorParser;
    protected override Action<SessionContext, Color?> ValueUpdater { get; } = (context, value) => context.GlyphColor = value;
}
