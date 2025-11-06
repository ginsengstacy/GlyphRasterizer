using GlyphRasterizer.Prompting.PromptAction;

namespace GlyphRasterizer.Prompting;

public readonly struct PromptResult
{
    public CommandType? CommandType { get; }
    public PromptActionType? PromptActionType { get; }
    public object? ParsedInputValue { get; }
    public string? ErrorMessage { get; }

    public PromptResult(CommandType commandType)
    {
        CommandType = commandType;
    }

    public PromptResult(PromptActionType promptActionType, object parsedInputValue)
    {
        PromptActionType = promptActionType;
        ParsedInputValue = parsedInputValue;
    }

    public PromptResult(PromptActionType promptActionType, string errorMessage)
    {
        PromptActionType = promptActionType;
        ErrorMessage = errorMessage;
    }
}