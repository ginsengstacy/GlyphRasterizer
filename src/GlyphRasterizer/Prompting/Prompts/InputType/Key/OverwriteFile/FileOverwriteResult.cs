namespace GlyphRasterizer.Prompting.Prompts.InputType.Key.OverwriteFile;

public readonly struct FileOverwriteResult(OverwriteMode overwriteMode, bool shouldSave)
{
    public OverwriteMode OverwriteMode { get; } = overwriteMode;
    public bool ShouldSave { get; } = shouldSave;
}
