using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.Key.Restart;

public sealed class RestartPrompt(RestartParser restartParser, PromptActionParser promptActionParser)
    : PromptBase<ConsoleKeyInfo, RestartPromptResultType?>(promptActionParser)
{
    protected override string Message => PromptMessages.Restart;
    protected override Func<ConsoleKeyInfo> GetInput => () => ConsoleHelpers.ReadKeyLine(intercept: true);
    protected override IPromptInputParser<ConsoleKeyInfo, RestartPromptResultType?> Parser { get; } = restartParser;
}
