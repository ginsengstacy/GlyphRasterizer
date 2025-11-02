using GlyphRasterizer.Lookup.Format.Image;
using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using System.Collections.Immutable;
using System.Windows.Media;

namespace GlyphRasterizer.Configuration;

public sealed class SessionContext
{
    public SessionContext(
        GlyphTypeface glyphTypeface,
        ImmutableList<Glyph> glyphs,
        string outputDirectory,
        Color? glyphColor,
        int? imageSize,
        ImmutableList<ImageFormat>? imageFormats
    )
    {
        GlyphTypeface = glyphTypeface;
        Glyphs = glyphs;
        OutputDirectory = outputDirectory;
        GlyphColor = glyphColor;
        ImageSize = imageSize;
        ImageFormats = imageFormats;
    }

    public SessionContext(ImmutableArray<IPrompt> promptOrder)
    {
        PromptOrder = promptOrder;
    }

    public readonly ImmutableArray<IPrompt> PromptOrder;

    public OverwriteMode OverwriteMode { get; set; } = OverwriteMode.AskAgain;

    public GlyphTypeface? GlyphTypeface { get; set; }
    public ImmutableList<Glyph>? Glyphs { get; set; }
    public Color? GlyphColor { get; set; }
    public int? ImageSize { get; set; }
    public ImmutableList<ImageFormat>? ImageFormats { get; set; }
    public string? OutputDirectory { get; set; }
}
