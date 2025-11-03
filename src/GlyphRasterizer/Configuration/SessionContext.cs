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
        ImmutableArray<Glyph> glyphs,
        string outputDirectory,
        Color glyphColor,
        int imageSize,
        ImmutableArray<ImageFormat> formats
    )
    {
        GlyphTypeface = glyphTypeface;
        Glyphs = glyphs;
        OutputDirectory = outputDirectory;
        GlyphColor = glyphColor;
        ImageSize = imageSize;
        Formats = formats;
    }

    public SessionContext(ImmutableArray<IPrompt> promptOrder)
    {
        PromptOrder = promptOrder;
    }

    public readonly ImmutableArray<IPrompt> PromptOrder;

    public OverwriteMode OverwriteMode { get; set; } = OverwriteMode.AskAgain;

    public GlyphTypeface? GlyphTypeface { get; set; }
    public ImmutableArray<Glyph>? Glyphs { get; set; }
    public Color? GlyphColor { get; set; }
    public int? ImageSize { get; set; }
    public ImmutableArray<ImageFormat>? Formats { get; set; }
    public string? OutputDirectory { get; set; }
}
