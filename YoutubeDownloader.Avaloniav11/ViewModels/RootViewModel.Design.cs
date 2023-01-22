namespace YoutubeDownloader.ViewModels;

#nullable disable
public class DesignTimeRootViewModel : RootViewModel
{
    public DesignTimeRootViewModel() : base()
    {
        Dashboard = new Components.DashboardViewModel(null!, null!, null!);
    }

    protected override void OnViewLoaded()
    {
        // prevent logic from RootViewModel
    }
}

partial class RootViewModel
{

    protected RootViewModel() { } // for Designer
}