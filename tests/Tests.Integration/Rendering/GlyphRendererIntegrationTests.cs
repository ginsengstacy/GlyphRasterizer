using FluentAssertions;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;
using GlyphRasterizer.Rendering;
using Resources;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tests.Integration.Rendering
{
    public class GlyphRendererIntegrationTests
    {
        private static readonly GlyphRenderer _glyphRenderer = new();

        private const string UncontainedGlyph = "𐀀";

        private static readonly string _unifontPath = ResourceHelper.GetFullPath("Fonts/Unifont.otf");
        private static readonly GlyphTypeface _unifont = new(new Uri(_unifontPath));

        [Fact]
        public void RenderGlyph_Should_ProduceFrozenBitmapWithCorrectDimensions_When_InputIsValid()
        {
            RenderTargetBitmap bitmap = _glyphRenderer.RenderGlyph(new Glyph("A"), _unifont, Colors.Black, 64);

            bitmap.Should().NotBeNull();
            bitmap.IsFrozen.Should().BeTrue();
            bitmap.PixelWidth.Should().Be(64);
            bitmap.PixelHeight.Should().Be(64);
        }

        [Fact]
        public void RenderGlyph_Should_ThrowInvalidOperation_When_GlyphIsUncontained()
        {
            Action action = () => _glyphRenderer.RenderGlyph(new Glyph(UncontainedGlyph), _unifont, Colors.Black, 64);
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetGlyphOutline_Should_ThrowInvalidOperation_When_GlyphIsUncontained()
        {
            Action action = () => _glyphRenderer.GetGlyphOutline(new Glyph(UncontainedGlyph), _unifont, 64);
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetGlyphOutline_Should_ReturnGeometryWithCorrectDimensions_When_InputIsValid()
        {
            Geometry geometry = _glyphRenderer.GetGlyphOutline(new Glyph("A"), _unifont, 64);
            geometry.Should().NotBeNull();
            geometry.Bounds.Width.Should().BeGreaterThan(0);
            geometry.Bounds.Height.Should().BeGreaterThan(0);
        }
    }
}
    