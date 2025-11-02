using FluentAssertions;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using Resources;
using Resources.Messages;
using System.Collections.Immutable;
using System.Text;
using System.Windows.Media;
using Tests.Common.Prompting.Parsing;

namespace Tests.Integration.Prompting.Parsing;

public sealed class GlyphParserIntegrationTests : ParserTestBase<GlyphParser, GlyphParseContext, ImmutableList<Glyph>?>
{
    protected override GlyphParser Parser { get; } = new();

    private const string UncontainedGlyph1 = "𐀀";
    private const string UncontainedGlyph2 = "𐀁";

    private static readonly string _unifontPath = ResourceHelpers.GetFullPath("Fonts/Unifont.otf");
    private static readonly GlyphTypeface _unifont = new(new Uri(_unifontPath));

    public static readonly TheoryData<string, string> UncontainedGlyphInput = new()
    {
        {
            UncontainedGlyph1,
            string.Format(
                ErrorMessages.UncontainedGlyphs_FormatString,
                _unifont.FamilyNames.Values.FirstOrDefault() ?? SentinelMessages.UnknownFontName,
                $"'{UncontainedGlyph1}'"
            )
        },
        {
            "A" + UncontainedGlyph1,
            string.Format(
                ErrorMessages.UncontainedGlyphs_FormatString,
                _unifont.FamilyNames.Values.FirstOrDefault() ?? SentinelMessages.UnknownFontName,
                $"'{UncontainedGlyph1}'"
            )
        },
        {
            UncontainedGlyph1 + UncontainedGlyph2,
            string.Format(
                ErrorMessages.UncontainedGlyphs_FormatString,
                _unifont.FamilyNames.Values.FirstOrDefault() ?? SentinelMessages.UnknownFontName,
                $"'{UncontainedGlyph1}', '{UncontainedGlyph2}'"
            )
        }
    };

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_Should_ReturnEmptyInputError_When_InputIsEmpty(string input) =>
        AssertParseFailure(new GlyphParseContext(input, _unifont), ErrorMessages.EmptyInput);

    [Theory]
    [MemberData(nameof(UncontainedGlyphInput))]
    public void TryParse_Should_ReturnUncontainedGlyphsError_When_InputContainsUncontainedGlyphs(string input, string expectedMessage) =>
        AssertParseFailure(new GlyphParseContext(input, _unifont), expectedMessage);

    [Fact]
    public void TryParse_Should_ThrowNullReference_When_TypefaceIsNull() =>
        FluentActions.Invoking(() => Parser.TryParse(new GlyphParseContext("A", null), out _, out _))
            .Should().Throw<NullReferenceException>();

    [Theory]
    [InlineData("A")]
    [InlineData("z")]
    [InlineData("0")]
    [InlineData("!")]
    [InlineData("~")]
    [InlineData("Ω")]
    [InlineData("Я")]
    [InlineData("字")]
    public void TryParse_Should_ReturnTrue_When_InputIsSingleBMPChar(string input) =>
        AssertParseSuccess(new GlyphParseContext(input, _unifont), [new Glyph(input)]);

    [Theory]
    [InlineData("🄯")]
    [InlineData("𠀋")]
    [InlineData("𲍿")]
    public void TryParse_Should_ReturnTrue_When_InputIsSingleSMPChar(string input) =>
        AssertParseSuccess(new GlyphParseContext(input, _unifont), [new Glyph(input)]);

    [Theory]
    [InlineData("é")]
    [InlineData("ö")]
    [InlineData("ñ")]
    public void TryParse_Should_ReturnTrue_When_InputIsComposedChar(string input) =>
        AssertParseSuccess(new GlyphParseContext(input, _unifont), [new Glyph(input)]);

    [Theory]
    [InlineData("ABC")]
    [InlineData("A B C")]
    [InlineData("あいう")]
    [InlineData("妈汉龙")]
    public void TryParse_Should_ReturnTrue_When_InputHasMultipleDistinctGlyphs(string input) =>
        AssertParseSuccess(
            new GlyphParseContext(input, _unifont),
            [.. input.Where(c => !char.IsWhiteSpace(c)).Select(c => new Glyph(c.ToString()))]
        );

    [Theory]
    [InlineData(" A")]
    [InlineData("A ")]
    public void TryParse_Should_ReturnTrue_When_InputHasExtraWhitespace(string input) =>
        AssertParseSuccess(new GlyphParseContext(input, _unifont), [new Glyph(input.Trim())]);

    [Theory]
    [InlineData("AAA")]
    [InlineData("AABBCC")]
    [InlineData(" A A ")]
    [InlineData("AA BB CC")]
    [InlineData("ああいいうう")]
    [InlineData("妈妈汉汉龙龙")]
    public void TryParse_Should_ReturnTrue_When_InputHasDuplicateGlyphs(string input)
    {
        Rune[] distinctRunes = [.. input.EnumerateRunes().Where(r => !Rune.IsWhiteSpace(r)).Distinct()];
        ImmutableList<Glyph> expectedGlyphs = [.. distinctRunes.Select(r => new Glyph(r.ToString()))];
        AssertParseSuccess(new GlyphParseContext(input, _unifont), expectedGlyphs);
    }

    [Fact]
    public void TryParse_Should_NormalizeCompositionallyEquivalentGlyphs_ToSameCanonicalForm()
    {
        const string composed = "é";
        const string decomposed = "e\u0301";
        AssertParseSuccess(
            new GlyphParseContext(composed + decomposed, _unifont),
            [new Glyph(composed)]
        );
    }

    [Theory]
    [InlineData("A")]
    public void TryParse_Should_ReturnTrue_When_InputConsistsOfThousandsOfGlyphs(string input) =>
        AssertParseSuccess(new GlyphParseContext(new string(input[0], 10_000), _unifont), [new Glyph(input)]);
}