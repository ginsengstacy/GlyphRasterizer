using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using ImageMagick;
using System.Collections.Immutable;
using System.Windows.Media;

namespace GlyphRasterizer.Configuration;

public sealed class SessionContext
{
    public SessionContext(
        GlyphTypeface typeface,
        ImmutableArray<Glyph> glyphs,
        string outputDirectory,
        Color color,
        ImmutableArray<MagickFormat> imageFormats
    )
    {
        Typeface = typeface;
        Glyphs = glyphs;
        OutputDirectory = outputDirectory;
        Color = color;
        ImageFormats = imageFormats;
    }

    public SessionContext(ImmutableArray<IPrompt> promptOrder)
    {
        PromptOrder = promptOrder;
    }

    public readonly ImmutableArray<IPrompt> PromptOrder;

    public OverwriteMode OverwriteMode { get; set; } = OverwriteMode.AskAgain;
    public bool ShouldSkipFontAndOutputDirectoryPrompts;

    public GlyphTypeface? Typeface { get; set; }
    public ImmutableArray<Glyph>? Glyphs { get; set; }
    public Color? Color { get; set; }
    public ImmutableArray<MagickFormat>? ImageFormats { get; set; }
    public string? OutputDirectory { get; set; }
}
