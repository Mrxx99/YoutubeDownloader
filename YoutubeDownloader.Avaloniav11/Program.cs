using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using YoutubeDownloader.Services;
using YoutubeDownloader.ViewModels.Framework;

namespace YoutubeDownloader;
internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        var services = new ServiceCollection();

        services.AddSingleton<SettingsService>();
        services.AddSingleton<UpdateService>();
        services.AddSingleton<IViewManager, ViewManager>();
        services.AddSingleton<DialogManager>();
        services.AddSingleton<IViewModelFactory, ViewModelFactory>();
        services.AddTransient<IClipboard>(sp => sp.GetRequiredService<IViewManager>().GetTopLevel()!.Clipboard!);
        services.AddTransient<IApplicationLifetime>(sp => App.Current!.ApplicationLifetime!);
        services.AddSingleton(sp => App.Current!.PlatformSettings!);

        var serviceProvider = services.BuildServiceProvider();

        return AppBuilder.Configure<App>(() => ActivatorUtilities.CreateInstance<App>(serviceProvider))
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
