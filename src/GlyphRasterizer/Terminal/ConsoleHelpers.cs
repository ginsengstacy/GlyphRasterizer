namespace GlyphRasterizer.Terminal;

internal static class ConsoleHelpers
{
    private static bool ColorsAreSupported() =>
        !(OperatingSystem.IsBrowser() || OperatingSystem.IsAndroid() || OperatingSystem.IsIOS() || OperatingSystem.IsTvOS())
        && !Console.IsOutputRedirected;

    internal static void SetTerminalForegroundRed()
    {
        if (ColorsAreSupported())
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
    }

    internal static void ResetTerminalForegroundColor()
    {
        if (ColorsAreSupported())
        {
            Console.ResetColor();
        }
    }

    internal static void WriteError(string errorMessage, params object[] arg)
    {
        ResetTerminalForegroundColor();
        SetTerminalForegroundRed();
        Console.Error.WriteLine(errorMessage, arg);
        ResetTerminalForegroundColor();
    }

    internal static ConsoleKeyInfo ReadKeyLine(bool intercept = false)
    {
        ConsoleKeyInfo key = Console.ReadKey(intercept);
        Console.WriteLine();
        return key;
    }

    internal static string ReadLineSafe() => Console.ReadLine() ?? string.Empty;
}
