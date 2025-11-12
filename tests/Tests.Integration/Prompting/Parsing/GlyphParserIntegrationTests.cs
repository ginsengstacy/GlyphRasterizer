using FluentAssertions;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using Resources;
using Resources.Messages;
using System.Collections.Immutable;
using System.Text;
using System.Windows.Media;
using Tests.Common.Prompting.Parsing;

namespace Tests.Integration.Prompting.Parsing;

public sealed class GlyphParserIntegrationTests : ParserTestBase<GlyphParser, string, Glyph[]?>
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
                _unifont.FamilyNames.Values.First() ?? SentinelStrings.UnknownFontName,
                $"'{UncontainedGlyph1}'"
            )
        },
        {
            "A" + UncontainedGlyph1,
            string.Format(
                ErrorMessages.UncontainedGlyphs_FormatString,
                _unifont.FamilyNames.Values.First() ?? SentinelStrings.UnknownFontName,
                $"'{UncontainedGlyph1}'"
            )
        },
        {
            UncontainedGlyph1 + UncontainedGlyph2,
            string.Format(
                ErrorMessages.UncontainedGlyphs_FormatString,
                _unifont.FamilyNames.Values.First() ?? SentinelStrings.UnknownFontName,
                $"'{UncontainedGlyph1}', '{UncontainedGlyph2}'"
            )
        }
    };

    [Theory]
    [MemberData(nameof(EmptyStringInput))]
    public void TryParse_ShouldFailWithEmptyInputMessage_WhenInputIsEmpty(string input) =>
        AssertParseFailure(input, ErrorMessages.EmptyInput, _unifont);

    [Theory]
    [MemberData(nameof(UncontainedGlyphInput))]
    public void TryParse_ShouldFailWithUncontainedGlyphsMessage_WhenInputGlyphIsUncontained(string input, string expectedMessage) =>
        AssertParseFailure(input, expectedMessage, _unifont);

    [Theory]
    [InlineData("A")]
    [InlineData("z")]
    [InlineData("0")]
    [InlineData("!")]
    [InlineData("~")]
    [InlineData("Ω")]
    [InlineData("Я")]
    [InlineData("字")]
    public void TryParse_ShouldSucceed_WhenInputIsSingleBMPChar(string input) =>
        AssertParseSuccess(input, [new Glyph(input)], _unifont);

    [Theory]
    [InlineData("🄯")]
    [InlineData("𠀋")]
    [InlineData("𲍿")]
    public void TryParse_ShouldSucceed_WhenInputIsSingleSMPChar(string input) =>
        AssertParseSuccess(input, [new Glyph(input)], _unifont);

    [Theory]
    [InlineData("é")]
    [InlineData("ö")]
    [InlineData("ñ")]
    public void TryParse_ShouldSucceed_WhenInputIsComposedChar(string input) =>
        AssertParseSuccess(input, [new Glyph(input)], _unifont);

    [Theory]
    [InlineData("ABC")]
    [InlineData("A B C")]
    [InlineData("あいう")]
    [InlineData("妈汉龙")]
    public void TryParse_ShouldSucceed_WhenInputIsMultipleDistinctGlyphs(string input) =>
        AssertParseSuccess(input, [.. input.Where(c => !char.IsWhiteSpace(c)).Select(c => new Glyph(c.ToString()))], _unifont);

    [Theory]
    [InlineData(" A")]
    [InlineData("A ")]
    public void TryParse_ShouldSucceed_WhenInputHasExtraWhitespace(string input) =>
        AssertParseSuccess(input, [new Glyph(input.Trim())], _unifont);

    [Theory]
    [InlineData("AAA")]
    [InlineData("AABBCC")]
    [InlineData(" A A ")]
    [InlineData("AA BB CC")]
    [InlineData("ああいいうう")]
    [InlineData("妈妈汉汉龙龙")]
    public void TryParse_ShouldSucceed_WhenInputIsDuplicateGlyphs(string input)
    {
        Rune[] distinctRunes = [.. input.EnumerateRunes().Where(r => !Rune.IsWhiteSpace(r)).Distinct()];
        Glyph[] expectedGlyphs = [.. distinctRunes.Select(r => new Glyph(r.ToString()))];
        AssertParseSuccess(input, expectedGlyphs, _unifont);
    }

    [Fact]
    public void TryParse_ShouldNormalizeCompositionallyEquivalentGlyphs_WhenInputIsComposedAndDecomposed()
    {
        const string composed = "é";
        const string decomposed = "e\u0301";
        AssertParseSuccess(composed + decomposed, [new Glyph(composed)], _unifont);
    }

    [Theory]
    [InlineData("A")]
    public void TryParse_ShouldSucceed_WhenInputIsLarge(string input) =>
        AssertParseSuccess(new string(input[0], 10_000), [new Glyph(input)], _unifont);
}
