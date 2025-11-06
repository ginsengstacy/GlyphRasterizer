using GlyphRasterizer.Prompting.Prompts.InputType.String.UnicodeChar;
using Resources.Messages;
using System.Collections.Immutable;
using System.Windows.Media;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class UnicodeCharParserUnitTests : ParserTestBase<UnicodeCharParser, string, ImmutableArray<UnicodeChar>?>
{
    protected override UnicodeCharParser Parser { get; } = new();

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) =>
        AssertParseFailure(input, ErrorMessages.EmptyInput, new GlyphTypeface());
}