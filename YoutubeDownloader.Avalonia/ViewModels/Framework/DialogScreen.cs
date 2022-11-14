using System;
using YoutubeDownloader.ViewModels;

namespace YoutubeDownloader.ViewModels.Framework;

public abstract class DialogScreen<T> : ViewModelBase
{
    public T? DialogResult { get; private set; }

    public event EventHandler? Closed;

    public void Close(T? dialogResult = default)
    {
        DialogResult = dialogResult;
        Closed?.Invoke(this, EventArgs.Empty);
    }
}

public abstract class DialogScreen : DialogScreen<bool?>
{
}