using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using Resources.Messages;
using System.Windows.Media;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class GlyphColorParserUnitTests : ParserTestBase<GlyphColorParser, string, Color?>
{
    protected override GlyphColorParser Parser => new();

    public static readonly TheoryData<string, Color?> NamedColors = new()
    {
        { "Blue", Colors.Blue },
        { "Red", Colors.Red },
        { "red", Colors.Red }, // lowercase
        { " Red ", Colors.Red } // extra whitespace
    };

    public static readonly TheoryData<string, Color?> HexColors = new()
    {
        { "#FF0000", Colors.Red }, // RGB
        { "#FFFF0000", Colors.Red }, // ARGB
        { "#ff0000", Colors.Red }, // lowercase
        { " #FF0000 ", Colors.Red }, // extra whitespace
        { "#80FF0000", Color.FromArgb(0x80, 0xFF, 0x00, 0x00) }, // alpha 50%      
    };

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) =>
        AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Theory]
    [InlineData("#GGHHII")] // non-hex chars
    [InlineData("#FF000")] // incomplete hex
    [InlineData("#FF00000FF")] // wrong length 
    [InlineData("FF0000")] // missing #
    [InlineData("UnknownColorName")]
    public void TryParse_Should_ReturnInvalidFormatError_When_InputIsInvalid(string input) =>
        AssertParseFailure(input, ErrorMessages.InvalidFormat);

    [Theory]
    [MemberData(nameof(NamedColors))]
    public void TryParse_Should_ReturnTrue_When_InputIsNamedColor(string input, Color? expectedValue) =>
        AssertParseSuccess(input, expectedValue);

    [Theory]
    [MemberData(nameof(HexColors))]
    public void TryParse_Should_ReturnTrue_When_InputIsHexColor(string input, Color? expectedValue) =>
        AssertParseSuccess(input, expectedValue);
}
