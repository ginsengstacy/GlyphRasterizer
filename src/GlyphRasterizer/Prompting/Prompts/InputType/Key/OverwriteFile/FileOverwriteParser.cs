namespace GlyphRasterizer.Prompting.Prompts.InputType.Key.OverwriteFile;

public sealed class FileOverwriteParser : IPromptInputParser<ConsoleKeyInfo, FileOverwriteResult?>
{
    public bool TryParse(ConsoleKeyInfo input, out FileOverwriteResult? value, out string? errorMessage, object? additionalContext = null)
    {
        errorMessage = null;

        if (input.Key == ConsoleKey.Y)
        {
            value = input.Modifiers.HasFlag(ConsoleModifiers.Control)
                ? new FileOverwriteResult(OverwriteMode.OverwriteAll, shouldSave: true)
                : new FileOverwriteResult(OverwriteMode.AskAgain, shouldSave: true);

            return true;
        }

        if (input.Key == ConsoleKey.N)
        {
            value = input.Modifiers.HasFlag(ConsoleModifiers.Control)
                ? new FileOverwriteResult(OverwriteMode.SkipAll, shouldSave: false)
                : new FileOverwriteResult(OverwriteMode.AskAgain, shouldSave: false);

            return true;
        }

        value = null;
        return false;
    }
}
