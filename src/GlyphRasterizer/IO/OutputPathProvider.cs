using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using System.Globalization;
using System.IO;

namespace GlyphRasterizer.IO;

public sealed class OutputPathProvider(OutputDirectoryParser outputDirectoryParser)
{
    public string GetOutputDirectory(Glyph glyph, SessionContext context)
    {
        string outputDirectory = context.OutputDirectory!;
        if (!outputDirectoryParser.TryParse(outputDirectory, out _, out string? errorMessage))
            throw new FileNotFoundException(errorMessage);

        UnicodeCategory category = char.GetUnicodeCategory(glyph.UnicodeValue[0]);
        int imageSize = context.ImageSize!.Value;
        string glyphColor = context.GlyphColor!.Value.ToString().ToUpper();

        return Path.Combine(
            outputDirectory,
            "Rasterized Glyphs",
            "Glyph Colors",
            glyphColor,
            "Image Sizes",
            $"{imageSize}x{imageSize}",
            $"{category}s"
        );
    }

    public string GetFilePath(Glyph glyph, string extension, SessionContext context)
    {
        string directory = GetOutputDirectory(glyph, context);
        string fileName = $"Glyph_{glyph.UnicodeLabel}{extension}";
        return Path.Combine(directory, fileName);
    }
}
