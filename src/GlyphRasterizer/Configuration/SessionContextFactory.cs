using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;

namespace GlyphRasterizer.Configuration;

public sealed class SessionContextFactory(
    FontPrompt fontPrompt,
    GlyphPrompt glyphPrompt,
    ColorPrompt colorPrompt,
    ImageFormatPrompt imageFormatPrompt,
    OutputDirectoryPrompt outputDirectoryPrompt
)
{
    public SessionContext CreateDefault()
    {
        IPrompt[] promptOrder =
        [
            fontPrompt,
            glyphPrompt,
            colorPrompt,
            imageFormatPrompt,
            outputDirectoryPrompt
        ];

        return new SessionContext(promptOrder);
    }
}
