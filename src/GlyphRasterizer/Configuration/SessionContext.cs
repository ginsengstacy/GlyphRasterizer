using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.UnicodeChar;
using ImageMagick;
using System.Collections.Immutable;
using System.Windows.Media;

namespace GlyphRasterizer.Configuration;

public sealed class SessionContext
{
    public SessionContext(
        GlyphTypeface typeface,
        ImmutableArray<UnicodeChar> unicodeChars,
        string outputDirectory,
        Color color,
        uint imageSize,
        ImmutableArray<MagickFormat> imageFormats
    )
    {
        Typeface = typeface;
        UnicodeChars = unicodeChars;
        OutputDirectory = outputDirectory;
        Color = color;
        ImageSize = imageSize;
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
    public ImmutableArray<UnicodeChar>? UnicodeChars { get; set; }
    public Color? Color { get; set; }
    public uint? ImageSize { get; set; }
    public ImmutableArray<MagickFormat>? ImageFormats { get; set; }
    public string? OutputDirectory { get; set; }
}
