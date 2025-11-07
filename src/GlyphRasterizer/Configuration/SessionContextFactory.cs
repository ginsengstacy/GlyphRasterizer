using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using System.Collections.Immutable;

namespace GlyphRasterizer.Configuration;

public sealed class SessionContextFactory(
    FontPrompt fontPrompt,
    GlyphPrompt glyphPrompt,
    ImageFormatPrompt imageFormatPrompt,
    ImageSizePrompt imageSizePrompt,
    ColorPrompt colorPrompt,
    OutputDirectoryPrompt outputDirectoryPrompt
)
{
    public SessionContext CreateDefault()
    {
        ImmutableArray<IPrompt> promptOrder = ImmutableArray.Create<IPrompt>(
            fontPrompt,
            glyphPrompt,
            imageFormatPrompt,
            imageSizePrompt,
            colorPrompt,
            outputDirectoryPrompt
        );

        return new SessionContext(promptOrder);
    }
}
