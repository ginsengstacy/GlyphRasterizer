using GlyphRasterizer.Configuration;

namespace GlyphRasterizer.Prompting.PromptAction;

public readonly struct PromptActionExecutionContext
{
    public SessionContext SessionContext { get; init; }
    public object? Value { get; init; }
    public string? Message { get; init; }
}