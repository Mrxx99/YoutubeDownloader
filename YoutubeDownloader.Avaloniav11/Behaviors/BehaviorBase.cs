using Avalonia;
using Avalonia.Xaml.Interactivity;
using PropertyChanged;

namespace YoutubeDownloader.Behaviors;

[DoNotNotify]
public class BehaviorBase<T> : Behavior<T> where T : AvaloniaObject
{
}
