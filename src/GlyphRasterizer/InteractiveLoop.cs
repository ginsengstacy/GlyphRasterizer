using GlyphRasterizer.Configuration;
using GlyphRasterizer.IO;
using GlyphRasterizer.Lookup.PromptAction;
using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.Prompts.InputType.Key.Restart;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using GlyphRasterizer.Rendering;
using Resources.Messages;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GlyphRasterizer;

public sealed class InteractiveLoop(PromptWizard promptWizard, RestartPrompt restartPrompt, OutputSaver outputSaver)
{
    public void Run()
    {
        SessionContext? previousContext = null;

        while (true)
        {
            WriteIntroduction();

            try
            {
                SessionContext currentContext = promptWizard.GetSessionContext(previousContext);

                foreach (Glyph glyph in currentContext.Glyphs!)
                {
                    GlyphTypeface glyphTypeface = currentContext.GlyphTypeface!;
                    Color glyphColor = currentContext.GlyphColor!.Value;
                    int imageSize = currentContext.ImageSize!.Value;

                    RenderTargetBitmap bitmap = GlyphRenderer.RenderGlyph(glyph, glyphTypeface, glyphColor, imageSize);
                    outputSaver.SaveBitmapAsEachSelectedFormat(glyph, bitmap, currentContext);
                }

                Console.WriteLine(InfoMessages.OperationComplete);
                HandleRestart(ref previousContext, currentContext);
            }
            catch (OperationCanceledException)
            {
                ExitProgram();
            }
            catch (Exception ex)
            {
                ConsoleHelpers.WriteError($"{ex.Message}, {ex.StackTrace}");
            }
        }
    }

    public void HandleRestart(ref SessionContext? previousContext, SessionContext currentContext)
    {
        while (true)
        {
            object? restartPromptResultTypeObj = restartPrompt.Execute().ParsedInputValue;

            if (restartPromptResultTypeObj is RestartPromptResultType restartResult)
            {
                switch (restartResult)
                {
                    case RestartPromptResultType.RestartWithPreviousContext:
                        RestartProgram(ref previousContext, currentContext);
                        break;
                    case RestartPromptResultType.RestartWithoutPreviousContext:
                        RestartProgram(ref previousContext);
                        break;
                    case RestartPromptResultType.Quit:
                        ExitProgram();
                        break;
                }

                break;
            }
        }
    }

    private static void WriteIntroduction()
    {
        Console.WriteLine(InfoMessages.AppHeader);
        Console.WriteLine(
            string.Format(
                InfoMessages.AvailableCommands_FormatString,
                string.Join(
                    " | ",
                    PromptActionDataLookup.Lookup.Select(kvp => kvp.Value.DisplayName)
                )
            )
        );
        Console.WriteLine();
    }

    private static void RestartProgram(ref SessionContext? previousSessionContext, SessionContext? currentSessionContext = null)
    {
        previousSessionContext = currentSessionContext;
        Console.Clear();
    }

    private static void ExitProgram()
    {
        Console.WriteLine(InfoMessages.ProgramExited);
        Environment.Exit(0);
    }
}
