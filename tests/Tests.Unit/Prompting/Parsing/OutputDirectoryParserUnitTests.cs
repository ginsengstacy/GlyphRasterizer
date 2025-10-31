using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using Resources.Messages;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class OutputDirectoryParserUnitTests : ParserTestBase<OutputDirectoryParser, string, string?>
{
    protected override OutputDirectoryParser Parser => new();

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) =>
        AssertParseFailure(input, ErrorMessages.EmptyInput);
}
