using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Platform.Storage.FileIO;
using DialogHostAvalonia;

namespace YoutubeDownloader.ViewModels.Framework;

public class DialogManager : IDisposable
{
    private readonly IViewManager _viewManager;
    private readonly SemaphoreSlim _dialogLock = new(1, 1);

    public DialogManager(IViewManager viewManager)
    {
        _viewManager = viewManager;
    }

    public async ValueTask<T?> ShowDialogAsync<T>(DialogScreen<T> dialogScreen)
    {
        var view = _viewManager.CreateAndBindViewForModelIfNecessary(dialogScreen);

        void OnDialogOpened(object? openSender, DialogOpenedEventArgs openArgs)
        {
            void OnScreenClosed(object? closeSender, EventArgs closeArgs)
            {
                openArgs.Session.Close();
                dialogScreen.Closed -= OnScreenClosed;
            }
            dialogScreen.Closed += OnScreenClosed;
        }

        await _dialogLock.WaitAsync();
        try
        {
            await DialogHost.Show(view!, OnDialogOpened);
            return dialogScreen.DialogResult;
        }
        finally
        {
            _dialogLock.Release();
        }
    }

    public async Task<string?> PromptSaveFilePath(string filter = "All files|*.*", string defaultFilePath = "")
    {
        var mainWindow = _viewManager.GetMainWindow();

        var filePickResult = await mainWindow.StorageProvider.SaveFilePickerAsync(new()
        {
            FileTypeChoices = ParseFileTypes(filter),
            SuggestedFileName = defaultFilePath,
            DefaultExtension = Path.GetExtension(defaultFilePath).TrimStart('.')
        });

        if (filePickResult?.TryGetUri(out var uri) ?? false)
        {
            return uri.LocalPath;
        }

        return null;


        IReadOnlyList<FilePickerFileType> ParseFileTypes(string filterString)
        {
            var filters = new List<FilePickerFileType>();

            var filterStrings = filterString.Split('|').Chunk(2);

            foreach (var filter in filterStrings)
            {
                if (filter.Length == 2)
                {
                    var extensions = filter[1].Split(";").ToArray();
                    filters.Add(new FilePickerFileType(filter[0]) { Patterns = extensions });
                }
            }

            return filters;
        }
    }

    public async Task<string?> PromptDirectoryPath(string defaultDirPath = "")
    {
        var mainWindow = _viewManager.GetMainWindow();

        var folderPickResult = await mainWindow.StorageProvider.OpenFolderPickerAsync(new()
        {
            AllowMultiple = false,
            SuggestedStartLocation = string.IsNullOrEmpty(defaultDirPath) ? null : new BclStorageFolder(defaultDirPath)
        });


        if (folderPickResult.FirstOrDefault()?.TryGetUri(out var uri) ?? false)
        {
            return uri.LocalPath;
        }

        return null;
    }

    public void Dispose()
    {
        _dialogLock.Dispose();
    }
}