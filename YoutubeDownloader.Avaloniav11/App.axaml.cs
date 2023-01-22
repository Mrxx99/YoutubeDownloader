using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using PropertyChanged;
using YoutubeDownloader.ViewModels;
using YoutubeDownloader.Views;
using YoutubeDownloader.Utils;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YoutubeDownloader.Services;

namespace YoutubeDownloader;

public partial class App
{
    private static Assembly Assembly { get; } = typeof(App).Assembly;

    public new static string Name { get; } = Assembly.GetName().Name!;

    public static Version Version { get; } = Assembly.GetName().Version!;

    public static string VersionString { get; } = Version.ToString(3);
}

[DoNotNotify]
public partial class App : Application
{
    private readonly IServiceProvider? _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // For Designer
#nullable disable
    public App()
    {
    }
#nullable restore

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (_serviceProvider is null)
        {
            return; // fix for Designer
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var rootViewModel = ActivatorUtilities.CreateInstance<RootViewModel>(_serviceProvider);

            desktop.MainWindow = new RootView
            {
                DataContext = rootViewModel,
            };

            Sanctions.Verify(desktop.MainWindow);
        }

        base.OnFrameworkInitializationCompleted();

        var settinsService = _serviceProvider.GetService<SettingsService>();

        settinsService?.Load();

        if (settinsService?.IsDarkModeEnabled ?? false)
        {
            SetDarkTheme();
        }
        else
        {
            SetLightTheme();
        }
    }

    private static Theme LightTheme { get; } = Theme.Create(
        Theme.Light,
        MediaColor.FromHex("#343838"),
        MediaColor.FromHex("#F9A825")
    );

    private static Theme DarkTheme { get; } = Theme.Create(
        Theme.Dark,
        MediaColor.FromHex("#E8E8E8"),
        MediaColor.FromHex("#F9A825")
    );

    public static void SetLightTheme()
    {
        var theme = App.Current!.LocateMaterialTheme<MaterialThemeBase>();
        theme.CurrentTheme = LightTheme;

        Current!.Resources["SuccessBrush"] = new SolidColorBrush(Colors.DarkGreen);
        Current!.Resources["CanceledBrush"] = new SolidColorBrush(Colors.DarkOrange);
        Current!.Resources["FailedBrush"] = new SolidColorBrush(Colors.DarkRed);
    }

    public static void SetDarkTheme()
    {
        var theme = App.Current!.LocateMaterialTheme<MaterialThemeBase>();
        theme.CurrentTheme = DarkTheme;

        Current!.Resources["SuccessBrush"] = new SolidColorBrush(Colors.LightGreen);
        Current!.Resources["CanceledBrush"] = new SolidColorBrush(Colors.Orange);
        Current!.Resources["FailedBrush"] = new SolidColorBrush(Colors.OrangeRed);
    }
}
