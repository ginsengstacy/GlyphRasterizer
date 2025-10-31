using GlyphRasterizer.Configuration;
using GlyphRasterizer.IO;
using GlyphRasterizer.Prompting;
using GlyphRasterizer.Prompting.PromptAction;
using GlyphRasterizer.Prompting.Prompts.InputType.Key.OverwriteFile;
using GlyphRasterizer.Prompting.Prompts.InputType.Key.Restart;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Font;
using GlyphRasterizer.Prompting.Prompts.InputType.String.GlyphColor;
using GlyphRasterizer.Prompting.Prompts.InputType.String.Glyphs;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageFormats;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using GlyphRasterizer.Prompting.Prompts.InputType.String.OutputDirectory;
using Microsoft.Extensions.DependencyInjection;

namespace GlyphRasterizer.DI;

public static class ServiceRegistration
{
    public static IServiceCollection AddGlyphRasterizer(this IServiceCollection services)
    {
        services
            .AddCoreServices()
            .AddPromptingSubsystem()
            .AddIoSubsystem();
        return services;
    }

    private static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<CLI>();
        services.AddSingleton<InteractiveLoop>();
        services.AddSingleton<SessionContextFactory>();
        services.AddSingleton<PromptWizard>();
        return services;
    }

    private static IServiceCollection AddPromptingSubsystem(this IServiceCollection services)
    {
        services
            .AddPromptServices()
            .AddParserServices()
            .AddValidationServices();
        return services;
    }

    private static IServiceCollection AddPromptServices(this IServiceCollection services)
    {
        services.AddSingleton<FontPrompt>();
        services.AddSingleton<GlyphsPrompt>();
        services.AddSingleton<GlyphColorPrompt>();
        services.AddSingleton<ImageSizePrompt>();
        services.AddSingleton<ImageFormatsPrompt>();
        services.AddSingleton<OutputDirectoryPrompt>();
        services.AddSingleton<FileOverwritePrompt>();
        services.AddSingleton<RestartPrompt>();
        return services;
    }

    private static IServiceCollection AddParserServices(this IServiceCollection services)
    {
        services.AddSingleton<GlyphTypefaceParser>();
        services.AddSingleton<GlyphsParser>();
        services.AddSingleton<GlyphColorParser>();
        services.AddSingleton<ImageFormatsParser>();
        services.AddSingleton<OutputDirectoryParser>();
        services.AddSingleton<FileOverwriteParser>();
        services.AddSingleton<RestartParser>();
        services.AddSingleton<IntParser>();
        services.AddSingleton<PromptActionParser>();
        return services;
    }

    private static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddSingleton<ImageSizeValidator>();
        return services;
    }

    private static IServiceCollection AddIoSubsystem(this IServiceCollection services)
    {
        services.AddSingleton<OutputPathProvider>();
        services.AddTransient<OutputSaver>();
        services.AddTransient<OverwriteDecisionService>();
        return services;
    }
}
