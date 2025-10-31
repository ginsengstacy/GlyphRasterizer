using GlyphRasterizer.Configuration;
using GlyphRasterizer.Exceptions;
using GlyphRasterizer.Prompting.Prompts.InputType.Key.OverwriteFile;
using System.IO;

namespace GlyphRasterizer.IO;

public sealed class OverwriteDecisionService(FileOverwritePrompt fileOverwritePrompt)
{
    public bool ShouldSave(string filePath, SessionContext context)
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
        object? fileOverwriteResultObj = fileOverwritePrompt.Execute().ParsedInputValue;

        if (fileOverwriteResultObj is FileOverwriteResult overwriteParseResult)
        {
            context.OverwriteMode = overwriteParseResult.OverwriteMode;
            return overwriteParseResult.ShouldSave;
        }

        throw new UnexpectedTypeException(typeof(FileOverwriteResult), fileOverwriteResultObj?.GetType());
    }
}
