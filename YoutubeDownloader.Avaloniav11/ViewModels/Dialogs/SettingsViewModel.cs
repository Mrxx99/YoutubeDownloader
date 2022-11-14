using System;
using YoutubeDownloader.Services;
using YoutubeDownloader.ViewModels.Framework;

namespace YoutubeDownloader.ViewModels.Dialogs;

public class SettingsViewModel : DialogScreen
{
    private readonly SettingsService _settingsService;

    //public bool IsAutoUpdateEnabled
    //{
    //    get => _settingsService.IsAutoUpdateEnabled;
    //    set => SetProperty(_settingsService.IsAutoUpdateEnabled, value, _settingsService, (s, n) => s.IsAutoUpdateEnabled = n);
    //}

    //public bool IsDarkModeEnabled
    //{
    //    get => _settingsService.IsDarkModeEnabled;
    //    set => SetProperty(_settingsService.IsDarkModeEnabled, value, _settingsService, (s, n) => s.IsDarkModeEnabled = n);
    //}

    //public bool ShouldInjectTags
    //{
    //    get => _settingsService.ShouldInjectTags;
    //    set => SetProperty(_settingsService.ShouldInjectTags, value, _settingsService, (s, n) => s.ShouldInjectTags = n);
    //}

    //public bool ShouldSkipExistingFiles
    //{
    //    get => _settingsService.ShouldSkipExistingFiles;
    //    set => SetProperty(_settingsService.ShouldSkipExistingFiles, value, _settingsService, (s, n) => s.ShouldSkipExistingFiles = n);
    //}

    //public string FileNameTemplate
    //{
    //    get => _settingsService.FileNameTemplate;
    //    set => SetProperty(_settingsService.FileNameTemplate, value, _settingsService, (s, n) => s.FileNameTemplate = n);
    //}

    //public int ParallelLimit
    //{
    //    get => _settingsService.ParallelLimit;
    //    set => SetProperty(_settingsService.ParallelLimit, Math.Clamp(value, 1, 10), _settingsService, (s, n) => s.ParallelLimit = n);
    //}

    public bool IsAutoUpdateEnabled
    {
        get => _settingsService.IsAutoUpdateEnabled;
        set => _settingsService.IsAutoUpdateEnabled = value;
    }

    public bool IsDarkModeEnabled
    {
        get => _settingsService.IsDarkModeEnabled;
        set => _settingsService.IsDarkModeEnabled = value;
    }

    public bool ShouldInjectTags
    {
        get => _settingsService.ShouldInjectTags;
        set => _settingsService.ShouldInjectTags = value;
    }

    public bool ShouldSkipExistingFiles
    {
        get => _settingsService.ShouldSkipExistingFiles;
        set => _settingsService.ShouldSkipExistingFiles = value;
    }

    public string FileNameTemplate
    {
        get => _settingsService.FileNameTemplate;
        set => _settingsService.FileNameTemplate = value;
    }

    public int ParallelLimit
    {
        get => _settingsService.ParallelLimit;
        set => _settingsService.ParallelLimit = Math.Clamp(value, 1, 10);
    }

    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }
}

public class TestViewModel : DialogScreen
{
    public string? Name { get; set; } = "Alex";

    private string? myVar;

    public string? MyPropertyvar
    {
        get { return myVar; }
        set { myVar = value; }
    }


    public int MyProperty { get; set; }
    public int MyProperty1 { get; set; }

    public TestViewModel()
    {
        myVar = "hu";
    }
}