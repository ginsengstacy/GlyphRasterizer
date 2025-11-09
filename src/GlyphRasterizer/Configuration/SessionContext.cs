using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using ImageMagick;
using System.Windows.Media;

namespace GlyphRasterizer.Configuration;

public sealed class SessionContext
{
    public SessionContext(GlyphTypeface typeface, Glyph[] glyphs, string outputDirectory, Color color, MagickFormat[] imageFormats)
    {
        Typeface = typeface;
        Glyphs = glyphs;
        OutputDirectory = outputDirectory;
        Color = color;
        ImageFormats = imageFormats;
    }

    public SessionContext(IPrompt[] promptOrder)
    {
        PromptOrder = promptOrder;
    }

    public readonly IPrompt[] PromptOrder = [];

    public OverwriteMode OverwriteMode { get; set; } = OverwriteMode.AskAgain;
    public bool ShouldSkipFontAndOutputDirectoryPrompts;

    public GlyphTypeface? Typeface { get; set; }
    public Glyph[]? Glyphs { get; set; }
    public Color? Color { get; set; }
    public MagickFormat[]? ImageFormats { get; set; }
    public string? OutputDirectory { get; set; }
}
