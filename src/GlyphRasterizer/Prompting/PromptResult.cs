using GlyphRasterizer.Prompting.PromptAction;

namespace GlyphRasterizer.Prompting;

public readonly struct PromptResult(PromptActionType promptActionType)
{
    public PromptActionType PromptActionType { get; } = promptActionType;
    public object? ParsedInputValue { get; }
    public string? ErrorMessage { get; }

    public PromptResult(PromptActionType promptActionType, object parsedInputValue) : this(promptActionType)
    {
        PromptActionType = promptActionType;
        ParsedInputValue = parsedInputValue;
    }

    public PromptResult(PromptActionType promptActionType, string errorMessage) : this(promptActionType)
    {
        PromptActionType = promptActionType;
        ErrorMessage = errorMessage;
    }
}