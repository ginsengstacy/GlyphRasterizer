using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Format;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using System.Collections.Immutable;

namespace GlyphRasterizer.Configuration;

public sealed class SessionContextFactory(
    FontPrompt typefacePrompt,
    GlyphPrompt glyphPrompt,
    GlyphColorPrompt glyphColorPrompt,
    ImageSizePrompt imageSizePrompt,
    ImageFormatPrompt imageFormatPrompt,
    OutputDirectoryPrompt outputDirectoryPrompt
)
{
    public SessionContext CreateDefault()
    {
        ImmutableArray<IPrompt> promptOrder = ImmutableArray.Create<IPrompt>(
            typefacePrompt,
            glyphPrompt,
            glyphColorPrompt,
            imageSizePrompt,
            imageFormatPrompt,
            outputDirectoryPrompt
        );

        return new SessionContext(promptOrder);
    }
}
