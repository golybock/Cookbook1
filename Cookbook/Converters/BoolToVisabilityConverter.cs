using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Cookbook.Converters;

[ValueConversion(typeof(bool), typeof(TextBlock))]
public class BoolToVisabilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return GetVisibility(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private object GetVisibility(object value)
    {
        if (!(value is bool))
            return Visibility.Collapsed;

        var objValue = (bool) value;

        if (!objValue)
            return Visibility.Collapsed;

        return Visibility.Visible;
    }
}