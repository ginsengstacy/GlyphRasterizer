using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;
using ImageMagick;
using Resources.Messages;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class ImageFormatParserUnitTests : ParserTestBase<ImageFormatParser, string, MagickFormat[]?>
{
    protected override ImageFormatParser Parser { get; } = new();

    private const string InvalidImageFormat = "InvalidImageFormat";
    private static readonly string _invalidFormatsErrorMessage = string.Format(ErrorMessages.InvalidFormats_FormatString, $"'{InvalidImageFormat}'");

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_ShouldFailWithReturnEmptyInputMessage_WhenInputIsEmpty(string input) => 
        AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Fact]
    public void TryParse_ShouldSucceed_WhenInputIsEveryAvailableFormat()
    {
        foreach (MagickFormat format in AppConfig.AvailableImageFormats)
        {
            AssertParseSuccess(format.ToString().ToLower(), [format]);
            AssertParseSuccess(format.ToString().ToUpper(), [format]);
            AssertParseSuccess($" {format} ", [format]);
        }
    }

    public static readonly TheoryData<string, MagickFormat[]> SingleAvailableFormat = new()
    {
        { "PNG", [MagickFormat.Png] },
        { "PNG,PNG", [MagickFormat.Png] } // duplicate input token
    };

    public static readonly TheoryData<string, MagickFormat[]> MultipleAvailableFormats = new()
    {
        { "PNG,JPEG", [MagickFormat.Png, MagickFormat.Jpeg] },     
        { "png,jpeg", [MagickFormat.Png, MagickFormat.Jpeg] },     // lowercase 
        { " PNG , JPEG ", [MagickFormat.Png, MagickFormat.Jpeg] }, // extra whitespace
        { "PNG,PNG,JPEG", [MagickFormat.Png, MagickFormat.Jpeg] }  // duplicate input token
    };

    public static readonly TheoryData<string> AvailableImageFormatsMixedWithInvalidImageFormat = new()
    {
        { $"{AppConfig.AvailableImageFormats.FirstOrDefault()},{InvalidImageFormat}" },   
        { $" {AppConfig.AvailableImageFormats.FirstOrDefault()} , {InvalidImageFormat} " } // extra whitespace
    };

    [Theory]
    [MemberData(nameof(SingleAvailableFormat))]
    public void TryParse_ShouldSucceed_WhenInputIsSingleAvailableFormat(string input, MagickFormat[] expectedValue) =>
        AssertParseSuccess(input, expectedValue);

    [Theory]
    [MemberData(nameof(MultipleAvailableFormats))]
    public void TryParse_ShouldSucceed_WhenInputIsMultipleAvailableFormats(string input, MagickFormat[] expectedValue) =>
        AssertParseSuccess(input, expectedValue);

    [Fact]
    public void TryParse_ShouldFailWithInvalidFormatsMessage_WhenInputIsSingleInvalidImageFormat() => 
        AssertParseFailure(InvalidImageFormat, _invalidFormatsErrorMessage);

    [Theory]
    [MemberData(nameof(AvailableImageFormatsMixedWithInvalidImageFormat))]
    public void TryParse_ShouldFailWithInvalidFormatsMessage_WhenInputMixesAvailableAndInvalidImageFormats(string input) =>
        AssertParseFailure(input, _invalidFormatsErrorMessage);
}
