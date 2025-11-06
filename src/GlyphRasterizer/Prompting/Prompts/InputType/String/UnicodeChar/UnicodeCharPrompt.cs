using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Terminal;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.UnicodeChar;

public sealed class UnicodeCharPrompt(UnicodeCharParser unicodeCharParser, CommandTypeParser commandTypeParser)
    : PromptBase<string, ImmutableArray<UnicodeChar>?>(commandTypeParser)
{
    protected override string Message => string.Format(PromptMessages.UnicodeChar_FormatString, Environment.NewLine);

    protected override Func<string> GetInput => () => ConsoleHelpers.ReadLineSafe();
    protected override IPromptInputParser<string, ImmutableArray<UnicodeChar>?> Parser { get; } = unicodeCharParser;
    protected override Action<SessionContext, ImmutableArray<UnicodeChar>?> ValueUpdater { get; } = (context, value) => context.UnicodeChars = value;
}