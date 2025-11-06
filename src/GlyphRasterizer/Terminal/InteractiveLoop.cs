using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Prompting.Prompts.InputType.Key.Restart;
using Resources.Messages;

namespace GlyphRasterizer.Terminal;

internal sealed class InteractiveLoop(PromptWizard promptWizard, RestartPrompt restartPrompt, GlyphProcessingOrchestrator glyphProcessingOrchestrator)
{
    internal void Run()
    {
        SessionContext? previousContext = null;

        while (true)
        {
            WriteIntroduction();

            try
            {
                SessionContext currentContext = promptWizard.GetSessionContext(previousContext);
                glyphProcessingOrchestrator.RenderAndSaveAllFromContext(currentContext);
                Console.WriteLine(InfoMessages.OperationComplete);
                HandleRestart(ref previousContext, currentContext);
            }
            catch (OperationCanceledException)
            {
                ExitProgram();
            }
            catch (Exception ex)
            {
                ConsoleHelpers.WriteError(ex.Message, ex.StackTrace ?? string.Empty);
            }
        }
    }

    internal void HandleRestart(ref SessionContext? previousContext, SessionContext currentContext)
    {
        while (true)
        {
            if (restartPrompt.Execute().ParsedInputValue is RestartPromptResultType restartResultType)
            {
                switch (restartResultType)
                {
                    case RestartPromptResultType.RestartWithPreviousFontAndOutputDirectory:
                        RestartProgram(ref previousContext, currentContext);
                        return;
                    case RestartPromptResultType.RestartWithoutPreviousFontAndOutputDirectory:
                        RestartProgram(ref previousContext);
                        return;
                    case RestartPromptResultType.Quit:
                        ExitProgram();
                        return;
                }
            }
        }
    }

    private static void WriteIntroduction()
    {
        Console.WriteLine(InfoMessages.AppHeader);
        Console.WriteLine(InfoMessages.AvailableCommands_FormatString, string.Join(" | ", Enum.GetNames<CommandType>()));
        Console.WriteLine();
    }

    private static void RestartProgram(ref SessionContext? previousContext, SessionContext? currentContext = null)
    {
        if (currentContext is not null)
        {
            currentContext.ShouldSkipFontAndOutputDirectoryPrompts = true;
        }

        previousContext = currentContext;
        Console.Clear();
    }

    private static void ExitProgram()
    {
        Console.WriteLine(InfoMessages.ProgramExited);
        Environment.Exit(0);
    }
}
