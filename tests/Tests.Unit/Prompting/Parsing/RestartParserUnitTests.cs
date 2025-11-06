using GlyphRasterizer.Prompting.Prompts.InputType.Key.Restart;
using Tests.Common.Prompting.Parsing;

namespace Tests.Unit.Prompting.Parsing;

public sealed class RestartParserUnitTests : ParserTestBase<RestartParser, ConsoleKeyInfo, RestartPromptResultType?>
{
    protected override RestartParser Parser { get; } = new();

    [Theory]
    [InlineData(ConsoleKey.Y, false, RestartPromptResultType.RestartWithoutPreviousFontAndOutputDirectory)]
    [InlineData(ConsoleKey.Y, true, RestartPromptResultType.RestartWithPreviousFontAndOutputDirectory)]
    [InlineData(ConsoleKey.N, false, RestartPromptResultType.Quit)]
    [InlineData(ConsoleKey.N, true, RestartPromptResultType.Quit)]
    public void TryParse_Should_ReturnTrue_When_InputIsValid(ConsoleKey key, bool ctrl, RestartPromptResultType expectedResult)
    {
        var input = new ConsoleKeyInfo(' ', key, shift: false, alt: false, control: ctrl);
        AssertParseSuccess(input, expectedResult);
    }

    [Theory]
    [InlineData(ConsoleKey.A)]
    [InlineData(ConsoleKey.Enter)]
    [InlineData(ConsoleKey.Escape)]
    [InlineData(ConsoleKey.Spacebar)]
    public void TryParse_Should_ReturnFalse_When_InputIsInvalid(ConsoleKey key)
    {
        var input = new ConsoleKeyInfo(' ', key, shift: false, alt: false, control: false);
        AssertParseFailure(input, null!);
    }
}
