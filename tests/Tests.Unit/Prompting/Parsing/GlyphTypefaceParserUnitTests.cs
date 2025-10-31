using GlyphRasterizer.Lookup.Format.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using Resources.Messages;
using System.IO;
using System.Windows.Media;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class GlyphTypefaceParserUnitTests : ParserTestBase<GlyphTypefaceParser, string, GlyphTypeface?>
{
    protected override GlyphTypefaceParser Parser => new();

    private static readonly string _invalidFilePath = Path.GetRandomFileName();
    private static readonly string _invalidFilePathWithoutExtension = Path.GetFileNameWithoutExtension(_invalidFilePath);
    private static readonly string _validFontExtension = FontFormatDataLookup.Lookup.Values.First().Extension;
    private static readonly string _invalidFilePathWithValidExtension = _invalidFilePathWithoutExtension + _validFontExtension;

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) =>
        AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Fact]
    public void TryParse_Should_ReturnInvalidFileExtensionError_When_FileExtensionIsInvalid() =>
        AssertParseFailure(_invalidFilePath, ErrorMessages.InvalidFileExtension);

    [Fact]
    public void TryParse_Should_ReturnPathNotFoundError_When_PathDoesNotExist() =>
        AssertParseFailure(_invalidFilePathWithValidExtension, ErrorMessages.PathNotFound);
}
