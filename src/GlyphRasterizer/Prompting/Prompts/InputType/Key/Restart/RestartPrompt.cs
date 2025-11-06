using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Terminal;
using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.Key.Restart;

public sealed class RestartPrompt(RestartParser restartParser, CommandTypeParser commandTypeParser)
    : PromptBase<ConsoleKeyInfo, RestartPromptResultType?>(commandTypeParser)
{
    protected override string Message => PromptMessages.Restart;
    protected override Func<ConsoleKeyInfo> GetInput => () => ConsoleHelpers.ReadKeyLine(intercept: true);
    protected override IPromptInputParser<ConsoleKeyInfo, RestartPromptResultType?> Parser { get; } = restartParser;
}
