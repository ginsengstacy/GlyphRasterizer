using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using Resources.Messages;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class UIntParserUnitTests : ParserTestBase<UIntParser, string, uint?>
{
    protected override UIntParser Parser { get; } = new();

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) => AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Theory]
    [InlineData("-1")]
    [InlineData("abc")]
    [InlineData("12a")]
    [InlineData("1.2")]
    [InlineData("-5-")]
    [InlineData("4294967296")]
    [InlineData("--1")]
    public void TryParse_Should_ReturnInvalidFormatError_When_InputIsInvalidUInt(string input) => AssertParseFailure(input, ErrorMessages.InvalidFormat);

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("4294967295", uint.MaxValue)]
    [InlineData(" 42 ", 42)]
    public void TryParse_Should_ReturnTrue_When_InputIsValidUInt(string input, uint expected) => AssertParseSuccess(input, expected);
}
