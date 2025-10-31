using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormats;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using System.Collections.Immutable;

namespace GlyphRasterizer.Configuration;

public sealed class SessionContextFactory(
    FontPrompt typefacePrompt,
    GlyphsPrompt glyphsPrompt,
    GlyphColorPrompt glyphColorPrompt,
    ImageSizePrompt imageSizePrompt,
    ImageFormatsPrompt imageFormatsPrompt,
    OutputDirectoryPrompt outputDirectoryPrompt
)
{
    public SessionContext CreateDefault()
    {
        ImmutableArray<IPrompt> promptOrder = ImmutableArray.Create<IPrompt>(
            typefacePrompt,
            glyphsPrompt,
            glyphColorPrompt,
            imageSizePrompt,
            imageFormatsPrompt,
            outputDirectoryPrompt
        );

        return new SessionContext(promptOrder);
    }
}
