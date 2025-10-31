using FluentAssertions;
using GlyphRasterizer.Extensions;

namespace Tests.Unit.Extensions;

public sealed class StringExtensionUnitTests
{
    [Theory]
    [InlineData("Hello World", "Hello World")] // exact match
    [InlineData("Hello John Doe!", "Hello {firstName} {lastName}!")] // basic placeholders
    [InlineData("A123B456C", "A{num1}B{num2}C")] // adjacent placeholders
    [InlineData("start middle end", "{prefix} middle {suffix}")] // leading/trailing placeholders
    [InlineData("foo bar foo bar", "foo {x} foo {y}")] // repeated literals
    [InlineData("anything", "{any}")] // only placeholders
    [InlineData("anything", "")] // empty format
    public void MatchesFormat_Should_ReturnTrue_When_LiteralsMatchInOrder(string actual, string format) =>
        actual.MatchesFormat(format).Should().BeTrue();

    [Theory]
    [InlineData("Hello John", "Hello {firstName} {lastName}")] // missing part
    [InlineData("Hello John", "Hello {firstName} World")] // extra literal in format
    [InlineData("second first", "first {something} second")] // wrong order
    [InlineData("", "Hello {name}")] // empty actual
    [InlineData("Hello World", "hello {something}")] // case sensitive
    public void MatchesFormat_Should_ReturnFalse_When_LiteralsDoNotMatch(string actual, string format) =>
        actual.MatchesFormat(format).Should().BeFalse();

    [Fact]
    public void MatchesFormat_Should_ThrowNullReference_When_ActualIsNull() =>
        FluentActions.Invoking(() => ((string?)null)!.MatchesFormat("format"))
            .Should().Throw<NullReferenceException>();

    [Fact]
    public void MatchesFormat_Should_ThrowArgumentNull_When_FormatIsNull() =>
        FluentActions.Invoking(() => "actual".MatchesFormat(null!))
            .Should().Throw<ArgumentNullException>();
}
