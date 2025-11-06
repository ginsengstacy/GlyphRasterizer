using GlyphRasterizer.Configuration;
using GlyphRasterizer.Output;
using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Prompting.Prompts.InputType.Key.OverwriteFile;
using GlyphRasterizer.Prompting.Prompts.InputType.Key.Restart;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormat;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyph;
using GlyphRasterizer.Terminal;
using Microsoft.Extensions.DependencyInjection;

namespace GlyphRasterizer.DI;

internal static class ServiceRegistration
{
    internal static IServiceCollection AddGlyphRasterizer(this IServiceCollection services) =>
        services
            .AddCoreServices()
            .AddPromptingSubsystem()
            .AddOutputSubsystem();

    private static IServiceCollection AddCoreServices(this IServiceCollection services) =>
        services
            .AddSingleton<CLI>()
            .AddSingleton<InteractiveLoop>()
            .AddSingleton<PromptWizard>()
            .AddSingleton<SessionContextFactory>()
            .AddSingleton<GlyphProcessingOrchestrator>();

    private static IServiceCollection AddPromptingSubsystem(this IServiceCollection services) =>
        services
            .AddPrompts()
            .AddParsers()
            .AddValidators();

    private static IServiceCollection AddPrompts(this IServiceCollection services) =>
        services
            .AddSingleton<ColorPrompt>()
            .AddSingleton<FileOverwritePrompt>()
            .AddSingleton<FontPrompt>()
            .AddSingleton<ImageFormatPrompt>()
            .AddSingleton<ImageSizePrompt>()
            .AddSingleton<OutputDirectoryPrompt>()
            .AddSingleton<RestartPrompt>()
            .AddSingleton<GlyphPrompt>();

    private static IServiceCollection AddParsers(this IServiceCollection services) =>
        services
            .AddSingleton<ColorParser>()
            .AddSingleton<CommandTypeParser>()
            .AddSingleton<FileOverwriteParser>()
            .AddSingleton<ImageFormatParser>()
            .AddSingleton<OutputDirectoryParser>()
            .AddSingleton<RestartParser>()
            .AddSingleton<TypefaceParser>()
            .AddSingleton<UIntParser>()
            .AddSingleton<GlyphParser>();

    private static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddSingleton<ImageSizeValidator>();

    private static IServiceCollection AddOutputSubsystem(this IServiceCollection services) =>
        services
            .AddSingleton<OutputSaver>()
            .AddSingleton<OverwriteDecisionService>();
}
