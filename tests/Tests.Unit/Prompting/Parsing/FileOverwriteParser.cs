using GlyphRasterizer.Prompting.Prompts.InputType.Key.OverwriteFile;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class FileOverwriteParserUnitTests : ParserTestBase<FileOverwriteParser, ConsoleKeyInfo, FileOverwriteResult?>
{
    protected override FileOverwriteParser Parser { get; } = new();

    [Theory]
    [InlineData(ConsoleKey.Y, false, OverwriteMode.AskAgain, true)]
    [InlineData(ConsoleKey.Y, true, OverwriteMode.OverwriteAll, true)]
    [InlineData(ConsoleKey.N, false, OverwriteMode.AskAgain, false)]
    [InlineData(ConsoleKey.N, true, OverwriteMode.SkipAll, false)]
    public void TryParse_ShouldSucceed_WhenInputKeyIsMapped(ConsoleKey key, bool ctrl, OverwriteMode expectedMode, bool expectedShouldSave)
    {
        var input = new ConsoleKeyInfo(' ', key, shift: false, alt: false, control: ctrl);
        var expectedResult = new FileOverwriteResult(expectedMode, expectedShouldSave);
        AssertParseSuccess(input, expectedResult);
    }

    [Theory]
    [InlineData(ConsoleKey.A)]
    [InlineData(ConsoleKey.Enter)]
    [InlineData(ConsoleKey.Escape)]
    public void TryParse_ShouldFail_WhenInputKeyIsNotMapped(ConsoleKey key)
    {
        var input = new ConsoleKeyInfo(' ', key, shift: false, alt: false, control: false);
        AssertParseFailure(input, null!);
    }
}
