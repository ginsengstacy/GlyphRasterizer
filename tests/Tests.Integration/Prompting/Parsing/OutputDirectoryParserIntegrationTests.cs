using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using System.IO;
using Tests.Common.Prompting.Parsing;

namespace Tests.Integration.Prompting.Parsing;

public sealed class OutputDirectoryPromptIntegrationTests : ParserTestBase<OutputDirectoryParser, string, string?>
{
    protected override OutputDirectoryParser Parser => new();

    private static readonly string _outputDirectory = Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

    public static readonly TheoryData<string> ValidOutputDirectory = new()
    {
        { _outputDirectory },
        { $"\"{_outputDirectory}\"" },
        { $"{_outputDirectory}\\" }
    };

    [Theory]
    [MemberData(nameof(ValidOutputDirectory))]
    public void TryParse_Should_ReturnTrue_When_OutputDirectoryIsValid(string outputDir) =>
        AssertParseSuccess(outputDir, _outputDirectory);
}