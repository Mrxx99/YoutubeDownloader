using System;
using Microsoft.Extensions.DependencyInjection;
using YoutubeDownloader.ViewModels.Components;
using YoutubeDownloader.ViewModels.Dialogs;

namespace YoutubeDownloader.ViewModels.Framework;

// Used to instantiate new view models while making use of dependency injection
public interface IViewModelFactory
{
    DashboardViewModel CreateDashboardViewModel();

    DownloadViewModel CreateDownloadViewModel();

    DownloadSingleSetupViewModel CreateDownloadSingleSetupViewModel();

    DownloadMultipleSetupViewModel CreateDownloadMultipleSetupViewModel();

    MessageBoxViewModel CreateMessageBoxViewModel();

    SettingsViewModel CreateSettingsViewModel();
}

public class ViewModelFactory : IViewModelFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ViewModelFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public DashboardViewModel CreateDashboardViewModel() => GetViewModel<DashboardViewModel>();

    public DownloadMultipleSetupViewModel CreateDownloadMultipleSetupViewModel() => GetViewModel<DownloadMultipleSetupViewModel>();

    public DownloadSingleSetupViewModel CreateDownloadSingleSetupViewModel() => GetViewModel<DownloadSingleSetupViewModel>();

    public DownloadViewModel CreateDownloadViewModel() => GetViewModel<DownloadViewModel>();

    public MessageBoxViewModel CreateMessageBoxViewModel() => GetViewModel<MessageBoxViewModel>();

    public SettingsViewModel CreateSettingsViewModel() => GetViewModel<SettingsViewModel>();

    private T GetViewModel<T>() => ActivatorUtilities.CreateInstance<T>(_serviceProvider);
}
