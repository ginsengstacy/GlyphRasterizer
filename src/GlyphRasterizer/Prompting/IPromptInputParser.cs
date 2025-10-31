namespace GlyphRasterizer.Prompting;

public interface IPromptInputParser<TInput, TValue>
{
    bool TryParse(TInput input, out TValue? value, out string? errorMessage);
}