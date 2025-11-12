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
    public void TryParse_ShouldFailWithEmptyInputMessage_WhenInputIsEmpty(string input) =>
        AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Fact]
    public void TryParse_ShouldFailWithPathNotFoundMessage_WhenPathDoesNotExist() => 
        AssertParseFailure(Path.GetRandomFileName(), ErrorMessages.PathNotFound);
}
