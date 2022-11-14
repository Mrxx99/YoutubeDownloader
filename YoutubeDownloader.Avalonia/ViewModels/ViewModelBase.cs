using System;
using Avalonia.Controls;
using ReactiveUI;

namespace YoutubeDownloader.ViewModels;

public partial class ViewModelBase : ReactiveObject, IViewAware
{
    private Control? _view;

    protected virtual void OnViewLoaded()
    {
    }

    protected virtual void OnClose()
    {
    }

    void IViewAware.AttachView(Control view)
    {
        _view = view;

        if (view is WindowBase window)
        {
            window.Initialized += _view_AttachedToVisualTree;
            window.Closed += _view_DetachedFromVisualTree;
        }
        else
        {
            _view.AttachedToVisualTree += _view_AttachedToVisualTree;
            _view.DetachedFromVisualTree += _view_DetachedFromVisualTree;
        }

        if (view.IsInitialized)
        {
            OnViewLoaded();
        }
    }

    private void _view_DetachedFromVisualTree(object? sender, EventArgs e)
    {
        if (_view is WindowBase window)
        {
            window.Initialized -= _view_AttachedToVisualTree;
            window.Closed -= _view_DetachedFromVisualTree;
        }
        else
        {
            _view!.AttachedToVisualTree += _view_AttachedToVisualTree;
            _view!.DetachedFromVisualTree += _view_DetachedFromVisualTree;
        }

        OnClose();
    }

    private void _view_AttachedToVisualTree(object? sender, EventArgs e)
    {
        OnViewLoaded();
    }
}

public interface IViewAware
{
    void AttachView(Control control);
}