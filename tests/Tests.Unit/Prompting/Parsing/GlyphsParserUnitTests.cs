using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;
using Resources.Messages;
using System.Collections.Immutable;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class GlyphsParserUnitTests : ParserTestBase<GlyphsParser, GlyphParseContext, ImmutableList<Glyph>?>
{
    protected override GlyphsParser Parser { get; } = new();

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) =>
        AssertParseFailure(new GlyphParseContext(input, glyphTypeface: null), ErrorMessages.EmptyInput);
}