namespace GlyphRasterizer.Prompting;

public interface IPromptValueValidator<TValue>
{
    bool IsValid(TValue? value, out string? errorMessage, object? additionalContext = null);
}
