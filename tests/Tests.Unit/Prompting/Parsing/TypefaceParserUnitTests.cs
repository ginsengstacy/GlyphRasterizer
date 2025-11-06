using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using Resources.Messages;
using System.IO;
using System.Windows.Media;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class TypefaceParserUnitTests : ParserTestBase<TypefaceParser, string, GlyphTypeface?>
{
    protected override TypefaceParser Parser => new();

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) => AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Fact]
    public void TryParse_Should_ReturnPathNotFoundError_When_PathDoesNotExist() => AssertParseFailure(Path.GetRandomFileName(), ErrorMessages.PathNotFound);
}
