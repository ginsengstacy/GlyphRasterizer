using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using System.Collections.Immutable;

namespace GlyphRasterizer.Prompting;

public sealed class PromptWizard(SessionContextFactory sessionContextFactory)
{
    public SessionContext GetSessionContext(SessionContext? previousContext)
    {
        SessionContext currentContext = previousContext ?? sessionContextFactory.CreateDefault();
        ImmutableArray<IPrompt> promptOrder = currentContext.PromptOrder;
        var totalSteps = promptOrder.Length;
        var i = 0;

        while (i < totalSteps)
        {
            IPrompt currentPrompt = promptOrder[i];

            if (currentContext.GlyphTypeface is not null && currentPrompt is FontPrompt
                || currentContext.OutputDirectory is not null && currentPrompt is OutputDirectoryPrompt)
            {
                i++;
                continue;
            }

            if (currentPrompt is GlyphPrompt glyphsPrompt)
            {
                glyphsPrompt.CurrentTypeface = currentContext.GlyphTypeface;
            }

            Console.Write($"[{i + 1}/{totalSteps}] ");
            PromptResult promptResult = currentPrompt.Execute();

            switch (promptResult.PromptActionType)
            {
                case PromptActionType.Continue:
                    currentPrompt.UpdateContextWithParsedAndValidValue(currentContext);
                    i++;
                    break;

                case PromptActionType.Retry:
                    ConsoleHelpers.WriteError(promptResult.ErrorMessage!);
                    break;

                case PromptActionType.GoBack:
                    i = Math.Max(0, i - 1);
                    break;

                case PromptActionType.Restart:
                    currentContext = sessionContextFactory.CreateDefault();
                    i = 0;
                    break;

                case PromptActionType.Quit:
                    throw new OperationCanceledException();
            }
        }

        return currentContext;
    }
}