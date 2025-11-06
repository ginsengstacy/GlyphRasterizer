namespace GlyphRasterizer.Prompting.Prompts.InputType.Key.Restart;

public sealed class RestartParser : IPromptInputParser<ConsoleKeyInfo, RestartPromptResultType?>
{
    public bool TryParse(ConsoleKeyInfo input, out RestartPromptResultType? value, out string? errorMessage, object? additionalContext = null)
    {
        errorMessage = null;

        if (input.Key == ConsoleKey.Y)
        {
            value = input.Modifiers.HasFlag(ConsoleModifiers.Control)
                ? RestartPromptResultType.RestartWithPreviousFontAndOutputDirectory
                : RestartPromptResultType.RestartWithoutPreviousFontAndOutputDirectory;

            return true;
        }

        if (input.Key == ConsoleKey.N)
        {
            value = RestartPromptResultType.Quit;
            return true;
        }

        value = null;
        return false;
    }
}
