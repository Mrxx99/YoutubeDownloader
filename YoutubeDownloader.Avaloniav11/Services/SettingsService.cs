using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Platform;
using Microsoft.Win32;
using Tyrrrz.Settings;
using YoutubeDownloader.Core.Downloading;
using YoutubeDownloader.ViewModels.Framework;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader.Services;

public partial class SettingsService : SettingsManager
{
    public bool IsAutoUpdateEnabled { get; set; } = true;

    public bool IsDarkModeEnabled { get; set; }

    public bool ShouldInjectTags { get; set; } = true;

    public bool ShouldSkipExistingFiles { get; set; }

    public string FileNameTemplate { get; set; } = "$title";

    public int ParallelLimit { get; set; } = 2;

    public Container LastContainer { get; set; } = Container.Mp4;

    public VideoQualityPreference LastVideoQualityPreference { get; set; } = VideoQualityPreference.Highest;

    public SettingsService(IPlatformSettings platformSettings)
    {
        Configuration.StorageSpace = StorageSpace.Instance;
        Configuration.SubDirectoryPath = "";
        Configuration.FileName = "Settings.dat";
        IsDarkModeEnabled = platformSettings.GetColorValues().ThemeVariant is PlatformThemeVariant.Dark;
    }
}