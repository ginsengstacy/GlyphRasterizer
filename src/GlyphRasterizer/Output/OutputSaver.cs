using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.Prompts.InputType.String.UnicodeChar;
using GlyphRasterizer.Terminal;
using ImageMagick;
using Resources.Messages;
using System.IO;

namespace GlyphRasterizer.Output;

internal sealed class OutputSaver(OverwriteDecisionService overwriteDecisionService)
{
    internal void TrySaveImageAsEachSelectedFormat(UnicodeChar unicodeChar, MagickImage image, SessionContext context)
    {
        Directory.CreateDirectory(context.OutputDirectory!);

        foreach (MagickFormat imageFormat in context.ImageFormats!)
        {
            string fileExtension = '.' + (Enum.GetName(imageFormat) ?? string.Empty).ToLower();
            string outputPath = Path.Combine(context.OutputDirectory!, $"Glyph_{unicodeChar.Label}{fileExtension}");

            if (!overwriteDecisionService.ShouldSave(outputPath, context))
            {
                Console.WriteLine(InfoMessages.SkippedRasterization_FormatString, Path.GetFileName(outputPath));
                continue;
            }

            try
            {
                using IMagickImage<byte> imageToWrite = image.Clone();

                if (MagickFormatSupportsAlpha(imageFormat))
                {
                    imageToWrite.Format = imageFormat;
                    imageToWrite.Write(outputPath);
                }
                else
                {
                    using MagickImage flattenedImage = FlattenImageForOpaqueFormat(imageToWrite, MagickColors.White);
                    flattenedImage.Format = imageFormat;
                    flattenedImage.Write(outputPath);
                }

                Console.WriteLine(InfoMessages.SavedFile_FormatString, Path.GetFileName(outputPath));
            }
            catch (Exception ex)
            {
                ConsoleHelpers.WriteError(ExceptionMessages.FailedToSave_FormatString, outputPath, ex.Message);
            }
        }
    }

    private static bool MagickFormatSupportsAlpha(MagickFormat format) => format is MagickFormat.Png
                                                                                 or MagickFormat.WebP
                                                                                 or MagickFormat.Tiff
                                                                                 or MagickFormat.Psd;

    private static MagickImage FlattenImageForOpaqueFormat(IMagickImage<byte> image, MagickColor backgroundColor)
    {
        var background = new MagickImage(backgroundColor, image.Width, image.Height);
        background.Composite(image, CompositeOperator.Over);
        return background;
    }
}
