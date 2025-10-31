using GlyphRasterizer.Configuration;
using GlyphRasterizer.IO;
using GlyphRasterizer.Lookup.Format.Font;
using GlyphRasterizer.Lookup.Format.Image;
using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormats;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using GlyphRasterizer.Rendering;
using Resources.Messages;
using System.Collections.Immutable;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer;

public sealed class CLI(
    GlyphColorParser glyphColorParser,
    GlyphsParser glyphsParser,
    GlyphTypefaceParser glyphTypefaceParser,
    ImageFormatsParser imageFormatsParser,
    OutputDirectoryParser outputDirectoryParser,
    ImageSizeValidator imageSizeValidator,
    OutputSaver outputSaver
)
{
    private static readonly string[] _imageFormatNames = Enum.GetNames<ImageFormat>();

    public void Run(string[] args)
    {
        var root = new RootCommand("GlyphRasterizer CLI");

        // Arguments
        var fontPathArg = new Argument<GlyphTypeface>("font")
        {
            Description = string.Format(
                CLIDescriptions.FontPath_FormatString,
                string.Join(", ", Enum.GetNames<FontFormat>())
            ),
            CustomParser = result => ParseWith(result, glyphTypefaceParser)
        };

        var glyphsArg = new Argument<string>("glyphs") { Description = CLIDescriptions.Glyphs };

        var outputDirectoryArg = new Argument<string>("output")
        {
            Description = CLIDescriptions.OutputDirectory,
            CustomParser = result => ParseWith(result, outputDirectoryParser)
        };

        // Options
        var colorOpt = new Option<Color?>("--color")
        {
            Description = CLIDescriptions.Color,
            DefaultValueFactory = _ => Defaults.Color,
            CustomParser = result => ParseWith(result, glyphColorParser)
        };

        var sizeOpt = new Option<int>("--size")
        {
            Description = string.Format(CLIDescriptions.Size_FormatString, Config.MinImageSize, Config.MaxImageSize),
            DefaultValueFactory = _ => Defaults.ImageSize,
        };
        sizeOpt.Validators.Add(result => ValidateWith(result, imageSizeValidator));

        var formatOpt = new Option<ImmutableList<ImageFormat>>("--format")
        {
            Description = string.Format(
                CLIDescriptions.ImageFormats_FormatString,
                _imageFormatNames.ElementAtOrDefault(0),
                _imageFormatNames.ElementAtOrDefault(1),
                string.Join(", ", _imageFormatNames)
            ),
            AllowMultipleArgumentsPerToken = true,
            DefaultValueFactory = _ => Defaults.ImageFormats,
            CustomParser = result => ParseWith(result, imageFormatsParser)
        };

        root.Add(fontPathArg);
        root.Add(glyphsArg);
        root.Add(outputDirectoryArg);
        root.Add(colorOpt);
        root.Add(sizeOpt);
        root.Add(formatOpt);

        root.SetAction(parseResult =>
        {
            GlyphTypeface typeface = parseResult.GetValue(fontPathArg)!;
            string glyphInput = parseResult.GetValue(glyphsArg)!;
            string outputDirectory = parseResult.GetValue(outputDirectoryArg)!;
            Color? color = parseResult.GetValue(colorOpt);
            int? size = parseResult.GetValue(sizeOpt);
            ImmutableList<ImageFormat>? formats = parseResult.GetValue(formatOpt);

            if (!glyphsParser.TryParse(
                new GlyphParseContext(glyphInput, typeface),
                out ImmutableList<Glyph>? glyphs,
                out string? errorMessage)
            )
            {
                ConsoleHelpers.WriteError(errorMessage!);
                return;
            }

            var context = new SessionContext(typeface, glyphs!, outputDirectory, color, size, formats);

            foreach (Glyph glyph in context.Glyphs!)
            {
                RenderTargetBitmap bitmap = GlyphRenderer.RenderGlyph(glyph, typeface, color!.Value, size.Value);
                outputSaver.SaveBitmapAsEachSelectedFormat(glyph, bitmap, context);
            }
        });

        root.Parse(args).Invoke();
    }

    private static TParseResult? ParseWith<TParseResult>(ArgumentResult result, IPromptInputParser<string, TParseResult?> parser)
    {
        string combinedToken = string.Join(",", result.Tokens.Select(t => t.Value));
        if (!parser.TryParse(combinedToken, out TParseResult? value, out string? errorMessage))
        {
            result.AddError(errorMessage!);
        }

        return value;
    }

    private static void ValidateWith<TValue>(OptionResult result, IPromptValueValidator<TValue?> validator)
    {
        TValue? value = result.GetValueOrDefault<TValue>();
        if (!validator.IsValid(value, out string? errorMessage))
        {
            result.AddError(errorMessage!);
        }
    }
}
