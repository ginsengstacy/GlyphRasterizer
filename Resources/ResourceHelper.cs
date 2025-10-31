namespace Resources;

public static class ResourceHelper
{
    public static string GetFullPath(string relativePath) => Path.Combine(AppContext.BaseDirectory, relativePath.Replace('/', Path.DirectorySeparatorChar));
}
