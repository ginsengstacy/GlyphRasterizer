using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using Resources;
using Resources.Messages;
using System.IO;
using System.Windows.Media;
using Tests.Common.Prompting.Parsing;

namespace Tests.Integration.Prompting.Parsing;

public sealed class TypefaceParserIntegrationTests : ParserTestBase<TypefaceParser, string, GlyphTypeface?>
{
    protected override TypefaceParser Parser => new();

    private static readonly string _unifontPath = ResourceHelpers.GetFullPath("Fonts/Unifont.otf");
    private static readonly GlyphTypeface _unifont = new(new Uri(_unifontPath));

    public static readonly TheoryData<string> ExistingValidFontPath = new()
    {
        { _unifontPath },
        { $"\"{_unifontPath}\"" },
        { $"{_unifontPath}\\" }    
    };

    [Theory]
    [MemberData(nameof(ExistingValidFontPath))]
    public void TryParse_Should_ReturnTrue_When_FontPathIsValid(string fontPath) => AssertParseSuccess(fontPath, _unifont);

    [Fact]
    public void TryParse_Should_ReturnFailedToLoadError_When_GlyphTypefaceCannotBeLoaded()
    {
        string tmpPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".ttf");

        try
        {
            File.WriteAllText(tmpPath, "Not a real font.");
            AssertParseFailure(tmpPath, ExceptionMessages.FailedToLoad_FormatString, errorMessageShouldMatchFormat: true);
        }
        finally
        {
            if (File.Exists(tmpPath))
            {
                try
                {
                    File.Delete(tmpPath);
                }
                catch
                {
                }
            }
        }
    }
}
