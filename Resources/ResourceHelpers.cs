namespace Resources;

public static class ResourceHelpers
{
    public static string GetFullPath(string relativePath) => Path.Combine(AppContext.BaseDirectory, relativePath.Replace('/', Path.DirectorySeparatorChar));
}
