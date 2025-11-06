using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;
using ImageMagick;
using Resources.Messages;
using System.Collections.Immutable;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class ImageFormatParserUnitTests : ParserTestBase<ImageFormatParser, string, ImmutableArray<MagickFormat>?>
{
    protected override ImageFormatParser Parser { get; } = new();

    private const string InvalidImageFormat = "InvalidImageFormat";
    private static readonly string _invalidFormatsErrorMessage = string.Format(ErrorMessages.InvalidFormats_FormatString, $"'{InvalidImageFormat}'");

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) => AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Fact]
    public void TryParse_Should_ReturnTrue_When_InputIsEachFormat()
    {
        foreach (MagickFormat format in AppConfig.AvailableImageFormats)
        {
            AssertParseSuccess(format.ToString().ToLower(), [format]);
            AssertParseSuccess(format.ToString().ToUpper(), [format]);
            AssertParseSuccess($" {format} ", [format]);
        }
    }

    public static readonly TheoryData<string, ImmutableArray<MagickFormat>> ValidInput = new()
        {
            { "PNG,JPEG", [MagickFormat.Png, MagickFormat.Jpeg] },     // multiple
            { "png,jpeg", [MagickFormat.Png, MagickFormat.Jpeg] },     // multiple + lowercase 
            { " PNG , JPEG ", [MagickFormat.Png, MagickFormat.Jpeg] }, // multiple + extra whitespace
            { "PNG,PNG", [MagickFormat.Png] }                          // duplicate input token
        };

    public static readonly TheoryData<string> InvalidInput = new()
        {
            { InvalidImageFormat },
            { $"{AppConfig.AvailableImageFormats.FirstOrDefault()},{InvalidImageFormat}" },    // valid + invalid
            { $" {AppConfig.AvailableImageFormats.FirstOrDefault()} , {InvalidImageFormat} " } // valid + invalid + extra whitespace
        };

    [Theory]
    [MemberData(nameof(ValidInput))]
    public void TryParse_Should_ReturnTrue_When_InputIsValid(string input, ImmutableArray<MagickFormat> expectedValue) =>
        AssertParseSuccess(input, expectedValue);

    [Theory]
    [MemberData(nameof(InvalidInput))]
    public void TryParse_Should_ReturnInvalidFormatsError_When_InputIsInvalid(string input) => AssertParseFailure(input, _invalidFormatsErrorMessage);
}
