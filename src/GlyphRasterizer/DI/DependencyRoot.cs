using Microsoft.Extensions.DependencyInjection;

namespace GlyphRasterizer.DI;

public static class DependencyRoot
{
    public static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddGlyphRasterizer();
        return services.BuildServiceProvider();
    }
}