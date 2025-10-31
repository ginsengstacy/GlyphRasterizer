using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;
using Resources.Messages;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer.Rendering;

public static class GlyphRenderer
{
    public static RenderTargetBitmap RenderGlyph(Glyph glyph, GlyphTypeface glyphTypeface, Color glyphColor, int imageSize)
    {
        Geometry outline = GetGlyphOutline(glyph, glyphTypeface, imageSize);
        TransformGroup transform = CreateCenteredTransform(outline, imageSize);
        DrawingVisual visual = DrawGlyphVisual(outline, transform, glyphColor);
        return RenderToBitmap(visual, imageSize);
    }

    public static Geometry GetGlyphOutline(Glyph glyph, GlyphTypeface font, int imageSize)
    {
        if (!font.CharacterToGlyphMap.TryGetValue(glyph.CodePoint, out ushort glyphIndex))
        {
            throw new InvalidOperationException(ExceptionMessages.GlyphNotFound);
        }

        return font.GetGlyphOutline(glyphIndex, imageSize, hintingEmSize: 1);
    }

    public static TransformGroup CreateCenteredTransform(Geometry outline, int targetSize)
    {
        Rect bounds = outline.Bounds;
        double scale = targetSize / Math.Max(bounds.Width, bounds.Height);
        double offsetX = (targetSize - bounds.Width * scale) / 2 - bounds.X * scale;
        double offsetY = (targetSize - bounds.Height * scale) / 2 - bounds.Y * scale;

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

    private static RenderTargetBitmap RenderToBitmap(Visual visual, int size)
    {
        const double DPI = 96; // 1 WPF unit per pixel
        var bitmap = new RenderTargetBitmap(size, size, DPI, DPI, Defaults.PixelFormat);
        bitmap.Render(visual);
        bitmap.Freeze();
        return bitmap;
    }
}
