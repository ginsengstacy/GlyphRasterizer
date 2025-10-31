using GlyphRasterizer.Configuration;

namespace GlyphRasterizer.Prompting;

public interface IPrompt
{
    PromptResult Execute();
    void UpdateContextWithParsedAndValidValue(SessionContext context);
}