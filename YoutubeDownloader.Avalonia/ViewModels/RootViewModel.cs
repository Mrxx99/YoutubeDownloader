using YoutubeDownloader.Services;
using YoutubeDownloader.ViewModels.Dialogs;
using YoutubeDownloader.ViewModels.Framework;

namespace YoutubeDownloader.ViewModels;
public class RootViewModel : ViewModelBase
{
    private DialogManager _dialogManager = new DialogManager(new ViewManager());
    private SettingsService _settingsService = new SettingsService();

    public string Greeting => "Welcome to Avalonia!";

    public async void OpenSettings()
    {
        await _dialogManager.ShowDialogAsync(new SettingsViewModel(_settingsService));
    }

    public async void SaveFile()
    {
        var s = await _dialogManager.PromptSaveFilePath("Text Files (*.txt)|*.txt|Image Files (*.bmp, *.jpg)|*.bmp;*.jpg|All Files (*.*)|*.*", "mytext.txt");
    }

    public async void OpenFolder()
    {
        var s = await _dialogManager.PromptDirectoryPath();
    }

    public async void OnViewFullyLoaded()
    {
        //await ShowWarInUkraineMessageAsync();
        //await CheckForUpdatesAsync();
    }

    protected override void OnViewLoaded()
    {
        base.OnViewLoaded();

        _settingsService.Load();

        if (_settingsService.IsDarkModeEnabled)
        {
            App.SetDarkTheme();
        }
        else
        {
            App.SetLightTheme();
        }
    }

    protected override void OnClose()
    {
        base.OnClose();

        //Dashboard.CancelAllDownloads();

        _settingsService.Save();
        //_updateService.FinalizeUpdate(false);
    }
}
