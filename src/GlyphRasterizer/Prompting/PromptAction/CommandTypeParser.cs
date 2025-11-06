using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting.PromptAction;

public sealed class CommandTypeParser : IPromptInputParser<string, CommandType?>
{
    private static readonly ImmutableDictionary<string, CommandType?> _actionTypeToRepresentationMap =
        new Dictionary<string, CommandType?>
        {
            ["back"] = CommandType.Back,
            ["undo"] = CommandType.Back,
            ["restart"] = CommandType.Restart,
            ["reload"] = CommandType.Restart,
            ["quit"] = CommandType.Quit,
            ["exit"] = CommandType.Quit,
        }.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

    public bool TryParse(string input, out CommandType? value, out string? errorMessage, object? additionalContext = null)
    {
        errorMessage = null;
        string trimmedInput = input.Trim();
        return _actionTypeToRepresentationMap.TryGetValue(trimmedInput, out value);
    }
}
