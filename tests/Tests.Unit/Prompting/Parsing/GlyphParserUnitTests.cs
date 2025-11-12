using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using Resources.Messages;
using System.Windows.Media;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class GlyphParserUnitTests : ParserTestBase<GlyphParser, string, Glyph[]?>
{
    protected override GlyphParser Parser { get; } = new();

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_ShouldFailWithEmptyInputMessage_WhenInputIsEmpty(string input) => 
        AssertParseFailure(input, ErrorMessages.EmptyInput, new GlyphTypeface());
}