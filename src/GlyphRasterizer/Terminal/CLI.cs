using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using ImageMagick;
using Resources.Messages;
using System.Collections.Immutable;
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
    ImageSizeValidator imageSizeValidator,
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
        var imageSizeOpt = new Option<uint?>("--size")
        {
            Description = string.Format(CLIDescriptions.ImageSize_FormatString, AppConfig.MinImageSize, AppConfig.MaxImageSize),
            DefaultValueFactory = _ => Defaults.ImageSize,
        };

        var colorOpt = new Option<Color?>("--color")
        {
            Description = CLIDescriptions.Color,
            DefaultValueFactory = _ => Defaults.Color,
            CustomParser = result => Parse(result, colorParser)
        };

        var formatOpt = new Option<ImmutableArray<MagickFormat>?>("--format")
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
        root.Add(imageSizeOpt);
        root.Add(formatOpt);

        root.SetAction(parseResult =>
        {
            GlyphTypeface typeface = parseResult.GetValue(fontArg)!;
            string rawGlyphs = parseResult.GetValue(glyphArg)!;
            string outputDirectory = parseResult.GetValue(outputDirectoryArg)!;
            Color? color = parseResult.GetValue(colorOpt);
            uint? imageSize = parseResult.GetValue(imageSizeOpt);
            ImmutableArray<MagickFormat>? imageFormats = parseResult.GetValue(formatOpt);

            if (!glyphParser.TryParse(rawGlyphs, out ImmutableArray<Glyph>? glyphs, out string? errorMessage, typeface))
            {
                ConsoleHelpers.WriteError(errorMessage!);
                return;
            }

            if (!imageSizeValidator.IsValid(imageSize, out errorMessage, imageFormats))
            {
                ConsoleHelpers.WriteError(errorMessage!);
                return;
            }

            var context = new SessionContext(typeface, glyphs!.Value, outputDirectory, color!.Value, imageSize!.Value, imageFormats!.Value);
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
