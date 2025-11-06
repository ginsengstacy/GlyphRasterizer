using Microsoft.Extensions.DependencyInjection;

namespace GlyphRasterizer.DI;

internal static class DependencyRoot
{
    internal static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddGlyphRasterizer();
        return services.BuildServiceProvider();
    }
}