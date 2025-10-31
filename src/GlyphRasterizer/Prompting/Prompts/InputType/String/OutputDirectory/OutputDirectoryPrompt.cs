using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;

public sealed class OutputDirectoryPrompt(OutputDirectoryParser outputDirectoryParser, PromptActionParser promptActionParser)
    : PromptBase<string, string?>(promptActionParser)
{
    protected override string Message => PromptMessages.OutputDirectory;
    protected override Func<string> GetInput => ConsoleHelpers.ReadLineSafe;
    protected override IPromptInputParser<string, string?> Parser { get; } = outputDirectoryParser;
    protected override Action<SessionContext, string?> ValueUpdater { get; } = (context, value) => context.OutputDirectory = value;
}
