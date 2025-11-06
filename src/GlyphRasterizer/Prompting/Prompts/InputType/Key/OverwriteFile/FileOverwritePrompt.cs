using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Terminal;
using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.Key.OverwriteFile;

public sealed class FileOverwritePrompt(FileOverwriteParser fileOverwriteParser, CommandTypeParser commandTypeParser)
    : PromptBase<ConsoleKeyInfo, FileOverwriteResult?>(commandTypeParser)
{
    protected override string Message => PromptMessages.FileAlreadyExists_FormatString;
    protected override Func<ConsoleKeyInfo> GetInput => () => ConsoleHelpers.ReadKeyLine(intercept: true);
    protected override IPromptInputParser<ConsoleKeyInfo, FileOverwriteResult?> Parser { get; } = fileOverwriteParser;
    protected override Action<SessionContext, FileOverwriteResult?> ValueUpdater { get; } =
        (context, value) => context.OverwriteMode = value!.Value.OverwriteMode;
}
