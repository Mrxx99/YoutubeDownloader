﻿using System;

namespace YoutubeDownloader.ViewModels.Framework;

public abstract class DialogScreen<T> : ViewModelBase
{
    public T? DialogResult { get; private set; }

    public event EventHandler? Closed;

    public void Close(T? dialogResult)
    {
        DialogResult = dialogResult;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    public void Close(object? dialogResult)
    {
        Close((T?)dialogResult);
    }
}

public abstract class DialogScreen : DialogScreen<bool?>
{
}