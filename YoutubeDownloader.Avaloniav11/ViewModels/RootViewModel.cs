using System;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using Material.Styles.Controls;
using Material.Styles.Models;
using YoutubeDownloader.Services;
using YoutubeDownloader.Utils;
using YoutubeDownloader.ViewModels.Components;
using YoutubeDownloader.ViewModels.Dialogs;
using YoutubeDownloader.ViewModels.Framework;

namespace YoutubeDownloader.ViewModels;
public partial class RootViewModel : ViewModelBase
{
    private readonly IViewModelFactory _viewModelFactory;
    private readonly DialogManager _dialogManager;
    private readonly SettingsService _settingsService;
    private readonly IViewManager _viewManager;
    private readonly UpdateService _updateService;
    private WindowNotificationManager? _notificationManager;

    public WindowNotificationManager? NotificationManager => _notificationManager ?? new WindowNotificationManager(_viewManager.GetMainWindow());

    private readonly TimeSpan _snackbarMessageDuration = TimeSpan.FromSeconds(5);

    public string DisplayName { get; set; }

    public DashboardViewModel Dashboard { get; protected set; }

    public RootViewModel(
        IViewModelFactory viewModelFactory,
        DialogManager dialogManager,
        SettingsService settingsService,
        IViewManager viewManager,
        UpdateService updateService)
    {
        _viewModelFactory = viewModelFactory;
        _dialogManager = dialogManager;
        _settingsService = settingsService;
        _viewManager = viewManager;
        _updateService = updateService;

        Dashboard = _viewModelFactory.CreateDashboardViewModel();

        DisplayName = $"{App.Name} v{App.VersionString}";
    }

    private async Task ShowWarInUkraineMessageAsync()
    {
        var dialog = _viewModelFactory.CreateMessageBoxViewModel(
            "Ukraine is at war!", @"
My country, Ukraine, has been invaded by Russian military forces in an act of aggression that can only be described as genocide.
Be on the right side of history! Consider supporting Ukraine in its fight for freedom.

Press LEARN MORE to find ways that you can help.".Trim(),
            "LEARN MORE", "CLOSE"
        );

        if (await _dialogManager.ShowDialogAsync(dialog) == true)
        {
            ProcessEx.StartShellExecute("https://tyrrrz.me/ukraine?source=youtubedownloader");
        }
    }

    // TODO
    private async Task CheckForUpdatesAsync()
    {
        try
        {
            var updateVersion = await _updateService.CheckForUpdatesAsync();
            if (updateVersion is null)
                return;

            SnackbarHost.Post(new SnackbarModel($"Downloading update to {App.Name} v{updateVersion}...", _snackbarMessageDuration) , null, DispatcherPriority.Normal);
            await _updateService.PrepareUpdateAsync(updateVersion);

            SnackbarHost.Post(new SnackbarModel(
                "Update has been downloaded and will be installed when you exit",
                TimeSpan.FromHours(9),
                new SnackbarButtonModel
                {
                    Text = "INSTALL NOW",
                    Action = () =>
                    {
                        _updateService.FinalizeUpdate(true);
                        // TODO
                        (App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.Shutdown();
                    }
                }),
                null, 
                DispatcherPriority.Normal);
        }
        catch
        {
            // Failure to update shouldn't crash the application
            NotificationManager?.Show(new Notification(null, "Failed to perform application update", NotificationType.Warning, TimeSpan.FromSeconds(5)));
        }
    }

    public async void OnViewFullyLoaded()
    {
        await ShowWarInUkraineMessageAsync();
        await CheckForUpdatesAsync();
    }

    protected override void OnViewLoaded()
    {
        OnViewFullyLoaded();
    }

    protected override void OnClose()
    {
        base.OnClose();

        Dashboard.CancelAllDownloads();

        _settingsService.Save();
        _updateService.FinalizeUpdate(false);
    }
}
