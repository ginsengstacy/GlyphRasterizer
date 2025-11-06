using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using ImageMagick;
using Resources.Messages;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer.Rendering;

public static class RenderingHelpers
{
    public static MagickImage RenderGlyph(Glyph glyph, GlyphTypeface typeface, Color color, uint imageSize)
    {
        Geometry outline = GetGlyphOutline(glyph, typeface, imageSize);
        TransformGroup transform = CreateCenteredTransform(outline, imageSize);
        DrawingVisual visual = DrawGlyphVisual(outline, transform, color);
        RenderTargetBitmap bitmap = RenderToBitmap(visual, (int)imageSize);

        using var memoryStream = new MemoryStream();
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        encoder.Save(memoryStream);
        memoryStream.Position = 0;
        return new MagickImage(memoryStream);
    }

    public static TransformGroup CreateCenteredTransform(Geometry outline, uint imageSize)
    {
        Rect bounds = outline.Bounds;
        double scale = imageSize / Math.Max(bounds.Width, bounds.Height);
        double offsetX = ((imageSize - (bounds.Width * scale)) / 2) - (bounds.X * scale);
        double offsetY = ((imageSize - (bounds.Height * scale)) / 2) - (bounds.Y * scale);

        return new TransformGroup
        {
            Children =
            {
                new ScaleTransform(scale, scale),
                new TranslateTransform(offsetX, offsetY)
            }
        };
    }

    public static DrawingVisual DrawGlyphVisual(Geometry outline, Transform transform, Color color)
    {
        var visual = new DrawingVisual();
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        transform.Freeze();

        using (DrawingContext drawingContext = visual.RenderOpen())
        {
            drawingContext.PushTransform(transform);
            drawingContext.DrawGeometry(brush, null, outline);
        }

        return visual;
    }

    private static Geometry GetGlyphOutline(Glyph glyph, GlyphTypeface font, uint imageSize)
    {
        return font.CharacterToGlyphMap.TryGetValue(glyph.CodePoint, out ushort glyphIndex)
            ? font.GetGlyphOutline(glyphIndex, imageSize, hintingEmSize: 1)
            : throw new InvalidOperationException(ExceptionMessages.GlyphNotFound);
    }

    private static RenderTargetBitmap RenderToBitmap(Visual visual, int imageSize)
    {
        const double DPI = 96; // 1 WPF unit per pixel
        var bitmap = new RenderTargetBitmap(imageSize, imageSize, DPI, DPI, AppConfig.PixelFormat);
        bitmap.Render(visual);
        bitmap.Freeze();
        return bitmap;
    }
}
