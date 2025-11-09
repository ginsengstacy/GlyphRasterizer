using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Terminal;
using Resources.Messages;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;

public sealed class GlyphPrompt(GlyphParser glyphParser, CommandTypeParser commandTypeParser)
    : PromptBase<string, ImmutableArray<Glyph>?>(commandTypeParser)
{
    protected override string Message => PromptMessages.Glyph;

    protected override Func<string> GetInput => () => ConsoleHelpers.ReadLineSafe();
    protected override IPromptInputParser<string, ImmutableArray<Glyph>?> Parser { get; } = glyphParser;
    protected override Action<SessionContext, ImmutableArray<Glyph>?> ValueUpdater { get; } = (context, value) => context.Glyphs = value;
}