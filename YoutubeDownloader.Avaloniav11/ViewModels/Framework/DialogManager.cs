﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
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
        var storageProvider = _viewManager.GetTopLevel()?.StorageProvider;

        if (storageProvider is null)
        {
            return null;
        }

        var filePickResult = await storageProvider.SaveFilePickerAsync(new()
        {
            FileTypeChoices = ParseFileTypes(filter),
            SuggestedFileName = defaultFilePath,
            DefaultExtension = Path.GetExtension(defaultFilePath).TrimStart('.')
        });

        if (filePickResult?.Path is Uri path)
        {
            return path.LocalPath;
        }

        return null;
    }

    public async Task<string?> PromptDirectoryPath(string defaultDirPath = "")
    {
        var storageProvider = _viewManager.GetTopLevel()?.StorageProvider;

        if (storageProvider is null)
        {
            return null;
        }

        var startLocation = await GetStorageFolder(storageProvider, defaultDirPath);
        var folderPickResult = await storageProvider.OpenFolderPickerAsync(new()
        {
            AllowMultiple = false,
            SuggestedStartLocation = startLocation
        });


        if (folderPickResult.FirstOrDefault()?.Path is Uri path)
        {
            return path.LocalPath;
        }

        return null;
    }

    public void Dispose()
    {
        _dialogLock.Dispose();
    }

    private static async Task<IStorageFolder?> GetStorageFolder(IStorageProvider storageProvider, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        var storageFolder = await storageProvider.TryGetFolderFromPathAsync(path);

        return storageFolder;
    }

    private static IReadOnlyList<FilePickerFileType> ParseFileTypes(string filterString)
    {
        var filters = new List<FilePickerFileType>();

        var filterStrings = filterString.Split('|').Chunk(2);

        foreach (var filter in filterStrings)
        {
            if (filter.Length == 2)
            {
                var extensions = filter[1].Split(";").ToArray();
                var appleTypeIdentifiers = extensions.Select(s => s.Replace("*.", "public.")).ToArray();
                filters.Add(new FilePickerFileType(filter[0])
                {
                    Patterns = extensions,
                    AppleUniformTypeIdentifiers = appleTypeIdentifiers
                });
            }
        }

        return filters;
    }
}