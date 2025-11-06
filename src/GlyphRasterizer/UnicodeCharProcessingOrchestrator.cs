using GlyphRasterizer.Configuration;
using GlyphRasterizer.Output;
using GlyphRasterizer.Prompting.Prompts.InputType.String.UnicodeChar;
using GlyphRasterizer.Rendering;
using ImageMagick;

namespace GlyphRasterizer;

internal class UnicodeCharProcessingOrchestrator(OutputSaver outputSaver)
{
    private readonly OutputSaver _outputSaver = outputSaver;

    internal void RenderAndSaveAllFromContext(SessionContext context)
    {
        foreach (UnicodeChar unicodeChar in context.UnicodeChars!)
        {
            MagickImage image = RenderingHelpers.RenderGlyph(
                unicodeChar, context.Typeface!,
                context.Color!.Value,
                context.ImageSize!.Value
            );

            _outputSaver.TrySaveImageAsEachSelectedFormat(unicodeChar, image, context);
            image.Dispose();
        }
    }
}
