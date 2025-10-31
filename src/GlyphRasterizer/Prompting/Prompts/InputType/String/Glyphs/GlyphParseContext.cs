using System.Windows.Media;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;

public readonly struct GlyphParseContext(string input, GlyphTypeface? glyphTypeface)
{
    public readonly string Input = input;
    public readonly GlyphTypeface? GlyphTypeface = glyphTypeface;
}