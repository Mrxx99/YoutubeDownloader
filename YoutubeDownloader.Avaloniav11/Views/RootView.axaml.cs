using Avalonia;

namespace YoutubeDownloader.Views;

public partial class RootView : ViewModelAwareWindow
{
    public RootView()
    {
        InitializeComponent();
        IsVisibleProperty.Changed.AddClassHandler<RootView>(OnWindowIsVisbleChanged);
        Loaded += RootView_Loaded;
        Opened += OnOpened;
    }

    private async void RootView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (IsVisible)
        {
            await Sanctions.Verify(this);
        }
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
