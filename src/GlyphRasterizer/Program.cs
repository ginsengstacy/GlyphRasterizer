using GlyphRasterizer.DI;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace GlyphRasterizer;

public static class Program
{
    static void Main(string[] args)
    {
        ServiceProvider provider = DependencyRoot.BuildServiceProvider();

        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.Unicode;

        if (args.Length > 0)
        {
            var cli = provider.GetRequiredService<CLI>();
            cli.Run(args);
        }
        else
        {
            var interactiveLoop = provider.GetRequiredService<InteractiveLoop>();
            interactiveLoop.Run();
        }
    }
}