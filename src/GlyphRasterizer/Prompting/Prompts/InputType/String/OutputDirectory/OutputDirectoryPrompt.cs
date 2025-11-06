using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Terminal;
using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;

public sealed class OutputDirectoryPrompt(OutputDirectoryParser outputDirectoryParser, CommandTypeParser commandTypeParser)
    : PromptBase<string, string?>(commandTypeParser)
{
    protected override string Message => PromptMessages.OutputDirectory;
    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, string?> Parser { get; } = outputDirectoryParser;
    protected override Action<SessionContext, string?> ValueUpdater { get; } = (context, value) => context.OutputDirectory = value;
}
