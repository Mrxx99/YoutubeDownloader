using Avalonia;
using Avalonia.Controls;
using PropertyChanged;

namespace YoutubeDownloader.Views;

[DoNotNotify]
public partial class RootView : Window
{
    public RootView()
    {
        InitializeComponent();
        IsVisibleProperty.Changed.AddClassHandler<RootView>(OnWindowIsVisbleChanged);
        //DialogHost.AttachedToVisualTree += RootView_AttachedToVisualTree;
        //DialogHost.DetachedFromVisualTree += DialogHost_DetachedFromVisualTree;
        Opened += OnOpened;
    }

    private async void OnOpened(object? sender, System.EventArgs e)
    {
        if (IsVisible)
        {
            //await Sanctions.Verify(this);
        }
    }

    //private void DialogHost_DetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    //{
    //    ((ViewModelBase)DataContext!).OnClose();
    //    DialogHost.AttachedToVisualTree -= RootView_AttachedToVisualTree;
    //    DialogHost.DetachedFromVisualTree -= DialogHost_DetachedFromVisualTree;
    //}

    //private void RootView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    //{
    //    ((ViewModelBase)DataContext!).OnViewLoaded();
    //}

    private async void OnWindowIsVisbleChanged(RootView window, AvaloniaPropertyChangedEventArgs args)
    {
        if (window.IsVisible)
        {
            //await Sanctions.Verify(this);
        }
    }
}
