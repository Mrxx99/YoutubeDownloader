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

namespace YoutubeDownloader;

[DoNotNotify]
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var rootViewModel = new RootViewModel();

            desktop.MainWindow = new RootView
            {
                DataContext = rootViewModel,
            };

            ((IViewAware)rootViewModel).AttachView(desktop.MainWindow);
        }

        base.OnFrameworkInitializationCompleted();

    }

    private static Theme LightTheme { get; } = Theme.Create(
        new MaterialDesignLightTheme(),
        MediaColor.FromHex("#343838"),
        MediaColor.FromHex("#F9A825")
    );

    private static Theme DarkTheme { get; } = Theme.Create(
        new MaterialDesignDarkTheme(),
        MediaColor.FromHex("#E8E8E8"),
        MediaColor.FromHex("#F9A825")
    );

    public static void SetLightTheme()
    {
        var paletteHelper = new PaletteHelper();
        paletteHelper.SetTheme(LightTheme);

        Current!.Resources["SuccessBrush"] = new SolidColorBrush(Colors.DarkGreen);
        Current!.Resources["CanceledBrush"] = new SolidColorBrush(Colors.DarkOrange);
        Current!.Resources["FailedBrush"] = new SolidColorBrush(Colors.DarkRed);
    }

    public static void SetDarkTheme()
    {
        var paletteHelper = new PaletteHelper();
        paletteHelper.SetTheme(DarkTheme);

        Current!.Resources["SuccessBrush"] = new SolidColorBrush(Colors.LightGreen);
        Current!.Resources["CanceledBrush"] = new SolidColorBrush(Colors.Orange);
        Current!.Resources["FailedBrush"] = new SolidColorBrush(Colors.OrangeRed);
    }
}
