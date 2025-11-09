using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using ImageMagick;
using Resources.Messages;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Windows.Media;

namespace GlyphRasterizer.Terminal;

internal sealed class CLI(
    ColorParser colorParser,
    GlyphParser glyphParser,
    TypefaceParser typefaceParser,
    ImageFormatParser imageFormatParser,
    OutputDirectoryParser outputDirectoryParser,
    GlyphProcessingOrchestrator glyphProcessingOrchestrator
)
{
    private static readonly string[] _imageFormatNames = [.. AppConfig.AvailableImageFormats.Select(f => Enum.GetName(f)!)];

    internal void Run(string[] args)
    {
        var root = new RootCommand("GlyphRasterizer CLI");

        // Arguments
        var fontArg = new Argument<GlyphTypeface?>("font")
        {
            Description = CLIDescriptions.Font,
            CustomParser = result => Parse(result, typefaceParser)
        };

        var glyphArg = new Argument<string?>("glyph")
        {
            Description = CLIDescriptions.Glyph
        };

        var outputDirectoryArg = new Argument<string?>("output")
        {
            Description = CLIDescriptions.OutputDirectory,
            CustomParser = result => Parse(result, outputDirectoryParser)
        };

        // Options
        var colorOpt = new Option<Color?>("--color")
        {
            Description = CLIDescriptions.Color,
            DefaultValueFactory = _ => Defaults.Color,
            CustomParser = result => Parse(result, colorParser)
        };

        var formatOpt = new Option<MagickFormat[]?>("--format")
        {
            Description = string.Format(
                CLIDescriptions.ImageFormat_FormatString,
                _imageFormatNames.ElementAtOrDefault(0),
                _imageFormatNames.ElementAtOrDefault(1),
                Environment.NewLine,
                string.Join(" | ", _imageFormatNames)
            ),
            AllowMultipleArgumentsPerToken = true,
            DefaultValueFactory = _ => Defaults.ImageFormats,
            CustomParser = result => Parse(result, imageFormatParser)
        };

        root.Add(fontArg);
        root.Add(glyphArg);
        root.Add(outputDirectoryArg);
        root.Add(colorOpt);
        root.Add(formatOpt);

        root.SetAction(parseResult =>
        {
            GlyphTypeface typeface = parseResult.GetValue(fontArg)!;
            string rawGlyphs = parseResult.GetValue(glyphArg)!;
            string outputDirectory = parseResult.GetValue(outputDirectoryArg)!;
            Color? color = parseResult.GetValue(colorOpt);
            MagickFormat[]? imageFormats = parseResult.GetValue(formatOpt);

            if (!glyphParser.TryParse(rawGlyphs, out Glyph[]? glyphs, out string? errorMessage, typeface))
            {
                ConsoleHelpers.WriteError(errorMessage!);
                return;
            }

            var context = new SessionContext(typeface, glyphs!, outputDirectory, color!.Value, imageFormats!);
            glyphProcessingOrchestrator.RenderAndSaveAllFromContext(context);
        });

        root.Parse(args).Invoke();
    }

    private static TParseResult? Parse<TParseResult>(ArgumentResult result, IPromptInputParser<string, TParseResult?> parser)
    {
        string combinedToken = string.Join(",", result.Tokens.Select(t => t.Value));
        if (!parser.TryParse(combinedToken, out TParseResult? value, out string? errorMessage))
        {
            result.AddError(errorMessage!);
        }

        return value;
    }
}
