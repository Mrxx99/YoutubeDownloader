using System;
using Avalonia.Controls;
using PropertyChanged;
using YoutubeDownloader.ViewModels;

namespace YoutubeDownloader.Views;

[DoNotNotify]
public class ViewModelAwareWindow : Window
{
    public ViewModelAwareWindow()
    {
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is IViewAware viewAware)
        {
            viewAware.AttachView(this);
        }
    }
}

[DoNotNotify]
public class ViewModelAwareUserControl : UserControl
{
    public ViewModelAwareUserControl()
    {
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is IViewAware viewAware)
        {
            viewAware.AttachView(this);
        }
    }
}
