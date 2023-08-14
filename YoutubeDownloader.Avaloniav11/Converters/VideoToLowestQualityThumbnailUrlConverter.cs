using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using YoutubeExplode.Videos;

namespace YoutubeDownloader.Converters;

public class VideoToLowestQualityThumbnailUrlConverter : IValueConverter
{
    public static VideoToLowestQualityThumbnailUrlConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        //return "https://img.youtube.com/vi/YQHsXMglC9A/default.jpg";
        if (value is IVideo video)
        {
            string? url = video.Thumbnails.OrderBy(t => t.Resolution.Area).FirstOrDefault()?.Url;
            return url;
        }
        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}