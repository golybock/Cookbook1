using System;
using System.Globalization;
using System.Windows.Data;

namespace Cookbook.Converters;

public class StringToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var ret = 0;
        return int.TryParse((string) value, out ret) ? ret : 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString();
    }
}