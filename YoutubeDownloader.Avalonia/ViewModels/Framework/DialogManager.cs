using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using DialogHost;

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
            await DialogHost.DialogHost.Show(view!, OnDialogOpened);
            return dialogScreen.DialogResult;
        }
        finally
        {
            _dialogLock.Release();
        }
    }

    public async Task<string?> PromptSaveFilePath(string filter = "All files|*.*", string defaultFilePath = "")
    {

        var dialog = new SaveFileDialog
        {
            Filters = ParseFilterString(filter),
            InitialFileName = defaultFilePath,
            DefaultExtension = Path.GetExtension(defaultFilePath).TrimStart('.')
        };

        return await dialog.ShowAsync(_viewManager.GetMainWindow());

        List<FileDialogFilter> ParseFilterString(string filterString)
        {
            var filters = new List<FileDialogFilter>();

            var filterStrings = filterString.Split('|').Chunk(2);

            foreach (var filter in filterStrings)
            {
                if (filter.Length == 2)
                {
                    var extensions = filter[1].Split(";").Select(s => s.Replace("*.", ""));
                    filters.Add(new FileDialogFilter { Name = filter[0], Extensions = extensions.ToList() });
                }
            }

            return filters;
        }
    }

    public async Task<string?> PromptDirectoryPath(string defaultDirPath = "")
    {
        var dialog = new OpenFolderDialog
        {
            Directory = defaultDirPath
        };

        return await dialog.ShowAsync(_viewManager.GetMainWindow());
    }

    public void Dispose()
    {
        _dialogLock.Dispose();
    }
}