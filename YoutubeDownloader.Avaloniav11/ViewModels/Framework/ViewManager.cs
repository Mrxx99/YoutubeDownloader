using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace YoutubeDownloader.ViewModels.Framework;

public interface IViewManager
{
    IControl? CreateAndBindViewForModelIfNecessary(object? model);
    Window GetMainWindow();
}

public class ViewManager : IViewManager
{
    private readonly ViewLocator _viewLocator = new();

    public IControl? CreateAndBindViewForModelIfNecessary(object? model)
    {
        var view = _viewLocator.Match(model) ? _viewLocator.Build(model) : null;

        if (view != null)
        {
            view.DataContext = model;
        }

        return view;
    }

    public Window GetMainWindow()
    {
        return ((IClassicDesktopStyleApplicationLifetime)App.Current!.ApplicationLifetime!).MainWindow!;
    }
}
