using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia;

namespace YoutubeDownloader;

public static class Sanctions
{
    //[ModuleInitializer]
    internal async static Task Verify(Window window)
    {
        var isSkipped = string.Equals(
            Environment.GetEnvironmentVariable("RUSNI"),
            "PYZDA",
            StringComparison.OrdinalIgnoreCase
        );

        if (isSkipped)
            return;

        var isSanctioned = new[]
        {
            CultureInfo.CurrentCulture,
            CultureInfo.CurrentUICulture,
            CultureInfo.InstalledUICulture,
            CultureInfo.DefaultThreadCurrentCulture,
            CultureInfo.DefaultThreadCurrentUICulture
        }.Any(c => true ||
            c is not null && (
                c.Name.Contains("-ru", StringComparison.OrdinalIgnoreCase) ||
                c.Name.Contains("-by", StringComparison.OrdinalIgnoreCase)
            )
        );

        if (!isSanctioned)
            return;

        var sanctionMessageBox = MessageBoxManager.GetMessageBoxStandardWindow(
            "Sanctioned region",
            "You cannot use this software on the territory of a terrorist state. " +
            "Set the environment variable `RUSNI=PYZDA` if you wish to override this check.",
            icon: MessageBox.Avalonia.Enums.Icon.Error);
        await sanctionMessageBox.ShowDialog(window);

        Environment.Exit(0xFACC);
    }
}