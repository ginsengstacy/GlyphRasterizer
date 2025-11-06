using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.Prompts.InputType.Key.OverwriteFile;
using System.IO;

namespace GlyphRasterizer.Output;

internal sealed class OverwriteDecisionService(FileOverwritePrompt fileOverwritePrompt)
{
    internal bool ShouldSave(string filePath, SessionContext context)
    {
        while (true)
        {
            if (!File.Exists(filePath) || context.OverwriteMode == OverwriteMode.OverwriteAll)
            {
                return true;
            }

            if (context.OverwriteMode == OverwriteMode.SkipAll)
            {
                return false;
            }

            fileOverwritePrompt.RuntimeMessageParameters = [Path.GetFileName(filePath)];

            if (fileOverwritePrompt.Execute().ParsedInputValue is FileOverwriteResult fileOverwriteResult)
            {
                context.OverwriteMode = fileOverwriteResult.OverwriteMode;
                return fileOverwriteResult.ShouldSave;
            }
        }
    }
}
