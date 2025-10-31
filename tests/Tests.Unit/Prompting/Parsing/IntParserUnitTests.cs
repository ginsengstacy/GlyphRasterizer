using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using Resources.Messages;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class IntParserUnitTests : ParserTestBase<IntParser, string, int?>
{
    protected override IntParser Parser { get; } = new();

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) =>
        AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Theory]
    [InlineData("abc")]
    [InlineData("12a")]
    [InlineData("1.2")]
    [InlineData("-5-")]
    [InlineData("2147483648")] // too large for int
    [InlineData("--1")]
    public void TryParse_Should_ReturnInvalidFormatError_When_InputIsInvalidInteger(string input) =>
        AssertParseFailure(input, ErrorMessages.InvalidFormat);

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("-42", -42)]
    [InlineData("2147483647", int.MaxValue)]
    [InlineData("-2147483648", int.MinValue)]
    [InlineData("  42  ", 42)] // extra spaces
    public void TryParse_Should_ReturnTrue_When_InputIsValidInteger(string input, int expected) =>
        AssertParseSuccess(input, expected);
}
