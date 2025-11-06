using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Terminal;
using Resources.Messages;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;

public sealed class ColorPrompt(ColorParser colorParser, CommandTypeParser commandTypeParser)
    : PromptBase<string, Color?>(commandTypeParser)
{
    protected override string Message => PromptMessages.Color;
    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, Color?> Parser { get; } = colorParser;
    protected override Action<SessionContext, Color?> ValueUpdater { get; } = (context, value) => context.Color = value;
}
