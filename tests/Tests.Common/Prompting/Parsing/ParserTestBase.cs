using FluentAssertions;
using GlyphRasterizer.Extensions;
using GlyphRasterizer.Prompting;

namespace Tests.Common.Prompting.Parsing;

public abstract class ParserTestBase<TParser, TInput, TValue>
    where TParser : IPromptInputParser<TInput, TValue>
    where TInput : notnull
{
    protected abstract TParser Parser { get; }

    public static readonly TheoryData<string> EmptyStringInput = new()
    {
        { "" },
        { " " },
        { "\t" },
        { "\n" }
    };

    protected void AssertParseSuccess(TInput input, TValue expectedValue)
    {
        bool success = Parser.TryParse(input, out TValue? parsed, out string? errorMessage);

        success.Should().BeTrue();
        parsed.Should().BeEquivalentTo(expectedValue);
        errorMessage.Should().BeNull();
    }

    protected void AssertParseFailure(TInput input, string expectedMessage, bool errorMessageShouldMatchFormat = false)
    {
        bool success = Parser.TryParse(input, out TValue? parsed, out string? errorMessage);

        success.Should().BeFalse();
        parsed.Should().BeNull();

        if (errorMessageShouldMatchFormat)
        {
            errorMessage!.MatchesFormat(expectedMessage).Should().BeTrue();
        }
        else
        {
            errorMessage.Should().Be(expectedMessage);
        }
    }
}
