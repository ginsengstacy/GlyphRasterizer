using GlyphRasterizer.Lookup.Format;
using GlyphRasterizer.Lookup.Format.Image;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormats;
using Resources.Messages;
using System.Collections.Immutable;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class ImageFormatsParserUnitTests : ParserTestBase<ImageFormatsParser, string, ImmutableList<ImageFormat>?>
{
    protected override ImageFormatsParser Parser { get; } = new();

    private static readonly ImmutableList<ImageFormat> _allImageFormats = [.. Enum.GetValues<ImageFormat>()];

    private const string InvalidImageFormat = "InvalidImageFormat";

    private static readonly string _invalidFormatsErrorMessage = string.Format(
        ErrorMessages.InvalidFormats_FormatString,
        $"'{InvalidImageFormat}'"
    );

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) =>
        AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Fact]
    public void TryParse_Should_ReturnTrue_When_InputIsEachFormatRepresentation()
    {
        foreach ((ImageFormat format, FormatLookupData formatData) in ImageFormatDataLookup.Lookup)
        {
            foreach (string representation in formatData.Representations)
            {
                AssertParseSuccess(representation.ToLowerInvariant(), [format]); // lowercase
                AssertParseSuccess(representation.ToUpperInvariant(), [format]); // uppercase
                AssertParseSuccess($" {representation} ", [format]); // extra whitespace
            }
        }
    }

    public static readonly TheoryData<string, ImmutableList<ImageFormat>> ValidInput =
        new()
        {
            { "PNG,JPG", [ ImageFormat.Png, ImageFormat.Jpeg] }, // multiple
            { "png,jpg", [ ImageFormat.Png, ImageFormat.Jpeg] }, // multiple lowercase 
            { " PNG , JPG ", [ ImageFormat.Png, ImageFormat.Jpeg] }, // multiple whitespace
            { "PNG,PNG", [ ImageFormat.Png ] }, // same representation
            { "JPG,JPEG", [ ImageFormat.Jpeg ] }, // different representation
            { "ALL", _allImageFormats }, // all
            { " ALL ", _allImageFormats }, // all extra whitespace
            { "all ", _allImageFormats } // all lowercase
        };

    public static readonly TheoryData<string> InvalidInput =
        new()
        {
            { InvalidImageFormat},
            { $"{_allImageFormats.FirstOrDefault()},{InvalidImageFormat}"}, // mixed valid + invalid
            { $" {_allImageFormats.FirstOrDefault()} , {InvalidImageFormat} "} // mixed valid + invalid + extra whitespace
        };

    [Theory]
    [MemberData(nameof(ValidInput))]
    public void TryParse_Should_ReturnTrue_When_InputIsValid(string input, ImmutableList<ImageFormat> expectedValue) =>
        AssertParseSuccess(input, expectedValue);

    [Theory]
    [MemberData(nameof(InvalidInput))]
    public void TryParse_Should_ReturnInvalidFormatsError_When_InputIsInvalid(string input) =>
        AssertParseFailure(input, _invalidFormatsErrorMessage);
}
