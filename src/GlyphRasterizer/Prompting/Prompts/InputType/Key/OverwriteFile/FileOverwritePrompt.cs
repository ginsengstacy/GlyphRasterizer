using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;

namespace GlyphRasterizer.Prompting.Prompts.InputType.Key.OverwriteFile;

public sealed class FileOverwritePrompt(FileOverwriteParser fileOverwriteParser, PromptActionParser promptActionParser)
    : PromptBase<ConsoleKeyInfo, FileOverwriteResult?>(promptActionParser)
{
    protected override string Message => PromptMessages.FileAlreadyExists_FormatString;
    protected override Func<ConsoleKeyInfo> GetInput => () => ConsoleHelpers.ReadKeyLine(intercept: true);
    protected override IPromptInputParser<ConsoleKeyInfo, FileOverwriteResult?> Parser { get; } = fileOverwriteParser;
    protected override Action<SessionContext, FileOverwriteResult?> ValueUpdater { get; } =
        (context, value) => context.OverwriteMode = value!.Value.OverwriteMode;
}
