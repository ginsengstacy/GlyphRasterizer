using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using GlyphRasterizer.Terminal;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting;

public sealed class PromptWizard(SessionContextFactory contextFactory)
{
    public SessionContext GetSessionContext(SessionContext? previousContext)
    {
        SessionContext currentContext = previousContext ?? contextFactory.CreateDefault();
        ImmutableArray<IPrompt> promptOrder = currentContext.PromptOrder;
        int totalSteps = promptOrder.Length;
        int i = 0;

        while (i < totalSteps)
        {
            IPrompt currentPrompt = promptOrder[i];

            if (ShouldSkipPrompt(currentContext, currentPrompt))
            {
                i++;
                continue;
            }

            if (currentPrompt is GlyphPrompt glyphPrompt)
            {
                glyphPrompt.AdditionalParsingContext = currentContext.Typeface!;
            }

            Console.Write($"[{i + 1}/{totalSteps}] ");
            PromptResult promptResult = currentPrompt.Execute();

            switch (promptResult.CommandType)
            {
                case CommandType.Back:
                    i = Math.Max(0, i - 1);
                    continue;

                case CommandType.Restart:
                    currentContext = contextFactory.CreateDefault();
                    i = 0;
                    continue;

                case CommandType.Quit:
                    throw new OperationCanceledException();
            }

            switch (promptResult.PromptActionType)
            {
                case PromptActionType.Continue:
                    currentPrompt.UpdateContextWithParsedAndValidValue(currentContext);
                    i++;
                    continue;

                case PromptActionType.Retry:
                    ConsoleHelpers.WriteError(promptResult.ErrorMessage!);
                    continue;
            }
        }

        return currentContext;
    }

    private static bool ShouldSkipPrompt(SessionContext context, IPrompt prompt) => context.ShouldSkipFontAndOutputDirectoryPrompts
                                                                                    && prompt is FontPrompt or OutputDirectoryPrompt;
}