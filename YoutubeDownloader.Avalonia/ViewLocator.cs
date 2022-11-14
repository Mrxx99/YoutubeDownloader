using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using YoutubeDownloader.ViewModels;

namespace YoutubeDownloader;
public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            var view = (Control)Activator.CreateInstance(type)!;

            if (data is IViewAware viewAware)
            {
                viewAware.AttachView(view);
            }

            return view;
        }
        else
        {
            return new TextBlock { Text = "Not Found: " + name };
        }
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}
