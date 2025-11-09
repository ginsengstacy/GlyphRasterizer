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
    public static MagickImage RenderGlyph(Glyph glyph, GlyphTypeface typeface, Color color)
    {
        Geometry outline = GetGlyphOutline(glyph, typeface, AppConfig.MaxImageSize);
        TransformGroup transform = CreateAutoFitTransform(outline);
        DrawingVisual visual = DrawGlyphVisual(outline, transform, color);

        double totalPadding = AppConfig.Padding * 2;
        int width = (int)Math.Ceiling(outline.Bounds.Width + totalPadding);
        int height = (int)Math.Ceiling(outline.Bounds.Height + totalPadding);
        RenderTargetBitmap bitmap = RenderToBitmap(visual, width, height);

        using var memoryStream = new MemoryStream();
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        encoder.Save(memoryStream);
        memoryStream.Position = 0;
        return new MagickImage(memoryStream);
    }

    private static Geometry GetGlyphOutline(Glyph glyph, GlyphTypeface typeface, double renderingEmSize)
    {
        return typeface.CharacterToGlyphMap.TryGetValue(glyph.CodePoint, out ushort glyphIndex)
            ? typeface.GetGlyphOutline(glyphIndex, renderingEmSize, hintingEmSize: 1)
            : throw new InvalidOperationException(ExceptionMessages.GlyphNotFound);
    }

    private static TransformGroup CreateAutoFitTransform(Geometry outline)
    {
        Rect bounds = outline.Bounds;
        double offsetX = -bounds.X + AppConfig.Padding;
        double offsetY = -bounds.Y + AppConfig.Padding;

        return new TransformGroup
        {
            Children =
            {
                new TranslateTransform(offsetX, offsetY)
            }
        };
    }

    private static DrawingVisual DrawGlyphVisual(Geometry outline, Transform transform, Color color)
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

    private static RenderTargetBitmap RenderToBitmap(Visual visual, int width, int height)
    {
        var bitmap = new RenderTargetBitmap(width, height, AppConfig.Dpi, AppConfig.Dpi, AppConfig.PixelFormat);
        bitmap.Render(visual);
        bitmap.Freeze();
        return bitmap;
    }
}
