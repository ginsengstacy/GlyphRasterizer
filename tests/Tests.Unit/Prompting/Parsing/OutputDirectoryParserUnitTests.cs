using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using Resources.Messages;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class OutputDirectoryParserUnitTests : ParserTestBase<OutputDirectoryParser, string, string?>
{
    protected override OutputDirectoryParser Parser => new();

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_ShouldFailWithEmptyInputMessage_WhenInputIsEmpty(string input) =>
        AssertParseFailure(input, ErrorMessages.EmptyInput);

    [Theory]
    [InlineData("C:\\FolderName\\SubfolderName")]
    public void TryParse_ShouldSucceed_WhenInputIsNotEmpty(string input) =>
        AssertParseSuccess(input, input);

    [Theory]
    [InlineData("\"C:\\FolderName\\SubfolderName\"", "C:\\FolderName\\SubfolderName")] // extra quotation mark
    [InlineData("\"C:\\FolderName\\SubfolderName\\\"", "C:\\FolderName\\SubfolderName")] // trailing backslash
    public void TryParse_ShouldReturnTrimmedOutputDirectory_WhenInputNeedsToBeTrimmed(string input, string expected) =>
        AssertParseSuccess(input, expected);
}
