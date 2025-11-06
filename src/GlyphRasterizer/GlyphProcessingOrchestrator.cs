using GlyphRasterizer.Configuration;
using GlyphRasterizer.Output;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using GlyphRasterizer.Rendering;
using ImageMagick;

namespace GlyphRasterizer;

internal class GlyphProcessingOrchestrator(OutputSaver outputSaver)
{
    private readonly OutputSaver _outputSaver = outputSaver;

    internal void RenderAndSaveAllFromContext(SessionContext context)
    {
        foreach (Glyph glyph in context.Glyphs!)
        {
            MagickImage image = RenderingHelpers.RenderGlyph(
                glyph, context.Typeface!,
                context.Color!.Value,
                context.ImageSize!.Value
            );

            _outputSaver.TrySaveImageAsEachSelectedFormat(glyph, image, context);
            image.Dispose();
        }
    }
}
