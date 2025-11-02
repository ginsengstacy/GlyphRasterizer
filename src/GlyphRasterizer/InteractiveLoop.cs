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
        SessionContext? previousSessionContext = null;

        while (true)
        {
            WriteIntroduction();

            try
            {
                SessionContext currentSessionContext = promptWizard.GetSessionContext(previousSessionContext);

                foreach (Glyph glyph in currentSessionContext.Glyphs!)
                {
                    GlyphTypeface glyphTypeface = currentSessionContext.GlyphTypeface!;
                    Color glyphColor = currentSessionContext.GlyphColor!.Value;
                    int imageSize = currentSessionContext.ImageSize!.Value;

                    RenderTargetBitmap bitmap = GlyphRenderer.RenderGlyph(glyph, glyphTypeface, glyphColor, imageSize);
                    outputSaver.SaveBitmapAsEachSelectedFormat(glyph, bitmap, currentSessionContext);
                }

                Console.WriteLine(InfoMessages.OperationComplete);
                HandleRestart(ref previousSessionContext, currentSessionContext);
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

    public void HandleRestart(ref SessionContext? previousSessionContext, SessionContext currentSessionContext)
    {
        while (true)
        {
            object? restartPromptResultTypeObj = restartPrompt.Execute().ParsedInputValue;

            if (restartPromptResultTypeObj is RestartPromptResultType restartResult)
            {
                switch (restartResult)
                {
                    case RestartPromptResultType.RestartWithPreviousContext:
                        RestartProgram(ref previousSessionContext, currentSessionContext);
                        break;
                    case RestartPromptResultType.RestartWithoutPreviousContext:
                        RestartProgram(ref previousSessionContext);
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
