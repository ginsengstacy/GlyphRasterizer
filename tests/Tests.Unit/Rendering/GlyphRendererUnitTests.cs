using FluentAssertions;
using GlyphRasterizer.Rendering;
using System.Windows;
using System.Windows.Media;

namespace Tests.Unit.Rendering;

public class GlyphRendererUnitTests
{
    private readonly static GlyphRenderer _glyphRenderer = new();

    [Fact]
    public void CreateCenteredTransform_Should_ScaleAndCenterGeometry()
    {
        var geometry = new RectangleGeometry(new Rect(0, 0, 10, 20));
        TransformGroup transform = _glyphRenderer.CreateCenteredTransform(geometry, 100);

        var scale = (ScaleTransform)transform.Children[0];
        var translate = (TranslateTransform)transform.Children[1];

        scale.ScaleX.Should().Be(5); // 100 / 20 = 5
        scale.ScaleY.Should().Be(5);
        translate.X.Should().Be(25); // (100 - 10 * 5) / 2 = 25
        translate.Y.Should().Be(0); // (100 - 20 * 5) / 2 = 0
    }

    [Fact]
    public void DrawGlyphVisual_Should_ReturnDrawingVisual()
    {
        var geometry = new RectangleGeometry(new Rect(0, 0, 10, 10));
        var scale = new ScaleTransform(1, 1);
        var color = Colors.Red;

        DrawingVisual drawingVisual = _glyphRenderer.DrawGlyphVisual(geometry, scale, color);
        drawingVisual.Should().NotBeNull();
    }
}
