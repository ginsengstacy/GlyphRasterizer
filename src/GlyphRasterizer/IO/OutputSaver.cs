using GlyphRasterizer.BitmapEncoders;
using GlyphRasterizer.Configuration;
using GlyphRasterizer.Lookup.Format.Image;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;
using Resources.Messages;
using System.IO;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer.IO;

public sealed class OutputSaver(OutputPathProvider outputPathProvider, OverwriteDecisionService overwriteDecisionService)
{
    public void SaveBitmapAsEachSelectedFormat(Glyph glyph, RenderTargetBitmap bitmap, SessionContext context)
    {
        Directory.CreateDirectory(outputPathProvider.GetOutputDirectory(glyph, context));

        foreach (ImageFormat imageFormat in context.ImageFormats!)
        {
            string extension = ImageFormatDataLookup.Lookup[imageFormat].Extension;
            string filePath = outputPathProvider.GetFilePath(glyph, extension, context);

            if (!overwriteDecisionService.ShouldSave(filePath, context))
            {
                Console.WriteLine(string.Format(InfoMessages.SkippedRasterization_FormatString, Path.GetFileName(filePath)));
                continue;
            }

            try
            {
                byte[] encodedBytes = BitmapEncoderHelpers.EncodeBitmapAs(bitmap, imageFormat);
                File.WriteAllBytes(filePath, encodedBytes);
                Console.WriteLine(string.Format(InfoMessages.SavedFile_FormatString, Path.GetFileName(filePath)));
            }
            catch (Exception ex)
            {
                ConsoleHelpers.WriteError(string.Format(ExceptionMessages.FailedToSave_FormatString, filePath, ex.Message));
            }
        }
    }
}
