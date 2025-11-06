using Resources.Messages;
using System.IO;

namespace GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;

public sealed class OutputDirectoryParser : IPromptInputParser<string, string?>
{
    public bool TryParse(string input, out string? value, out string? errorMessage, object? additionalContext = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = ErrorMessages.EmptyInput;
            return false;
        }

        string trimmedInput = input.Trim('"');
        string trimmedOutputDirectoryPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(trimmedInput));
        Directory.CreateDirectory(trimmedOutputDirectoryPath);

        value = trimmedOutputDirectoryPath;
        errorMessage = null;
        return true;
    }
}
