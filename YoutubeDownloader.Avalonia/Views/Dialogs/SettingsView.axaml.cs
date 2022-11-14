using Avalonia.Controls;
using Avalonia.Interactivity;
using PropertyChanged;

namespace YoutubeDownloader.Views.Dialogs;

[DoNotNotify]
public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void DarkModeToggleButton_Checked(object sender, RoutedEventArgs args) =>
        App.SetDarkTheme();

    private void DarkModeToggleButton_Unchecked(object sender, RoutedEventArgs args) =>
        App.SetLightTheme();
}