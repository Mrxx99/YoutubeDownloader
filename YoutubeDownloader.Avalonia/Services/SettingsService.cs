using System;
using System.Diagnostics;
using Microsoft.Win32;
using ReactiveUI.Fody.Helpers;
using Tyrrrz.Settings;
using YoutubeDownloader.Core.Downloading;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader.Services;

public partial class SettingsService : SettingsManager
{
    public bool IsAutoUpdateEnabled { get; set; } = true;

    public bool IsDarkModeEnabled { get; set; } = IsDarkModeEnabledByDefault();

    public bool ShouldInjectTags { get; set; } = true;

    public bool ShouldSkipExistingFiles { get; set; }

    public string FileNameTemplate { get; set; } = "$title";

    public int ParallelLimit { get; set; } = 2;

    public Container LastContainer { get; set; } = Container.Mp4;

    public VideoQualityPreference LastVideoQualityPreference { get; set; } = VideoQualityPreference.Highest;

    public SettingsService()
    {
        Configuration.StorageSpace = StorageSpace.Instance;
        Configuration.SubDirectoryPath = "";
        Configuration.FileName = "Settings.dat";
    }
}

public partial class SettingsService
{
    private static bool IsDarkModeEnabledByDefault()
    {
        if (OperatingSystem.IsWindows())
        {
            try
            {
                return Registry.CurrentUser.OpenSubKey(
                    "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
                    false
                )?.GetValue("AppsUseLightTheme") is 0;
            }
            catch
            {
                return false;
            }
        }
        else if (OperatingSystem.IsLinux())
        {
            try
            {
                string? theme = "light";

                Process p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "gsettings",
                        Arguments = "get org.gnome.desktop.interface gtk-theme",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                p.Start();

                while (!p.StandardOutput.EndOfStream)
                    theme += p.StandardOutput.ReadLine();

                p.WaitForExit();

                return theme?.Contains("dark", StringComparison.OrdinalIgnoreCase) ?? false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        return false;
    }
}