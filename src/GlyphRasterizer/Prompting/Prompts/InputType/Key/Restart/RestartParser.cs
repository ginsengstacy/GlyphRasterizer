namespace GlyphRasterizer.Prompting.Prompts.InputType.Key.Restart;

public sealed class RestartParser : IPromptInputParser<ConsoleKeyInfo, RestartPromptResultType?>
{
    public bool TryParse(ConsoleKeyInfo input, out RestartPromptResultType? value, out string? errorMessage)
    {
        errorMessage = null;

        if (input.Key == ConsoleKey.Y)
        {
            value = input.Modifiers.HasFlag(ConsoleModifiers.Control)
                ? RestartPromptResultType.RestartWithPreviousContext
                : RestartPromptResultType.RestartWithoutPreviousContext;

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
