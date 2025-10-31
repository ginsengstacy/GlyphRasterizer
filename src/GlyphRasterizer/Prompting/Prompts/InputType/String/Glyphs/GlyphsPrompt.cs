using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using Resources.Messages;
using System.Collections.Immutable;
using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;

public sealed class GlyphsPrompt(GlyphsParser glyphsParser, PromptActionParser promptActionParser)
    : PromptBase<GlyphParseContext, ImmutableList<Glyph>?>(promptActionParser)
{
    public GlyphTypeface? CurrentTypeface;

    protected override string Message => string.Format(PromptMessages.Glyphs_FormatString, Environment.NewLine);

    protected override Func<GlyphParseContext> GetInput => () => new(ConsoleHelpers.ReadLineSafe(), CurrentTypeface);
    protected override IPromptInputParser<GlyphParseContext, ImmutableList<Glyph>?> Parser { get; } = glyphsParser;
    protected override Action<SessionContext, ImmutableList<Glyph>?> ValueUpdater { get; } = (context, value) => context.Glyphs = value;
}