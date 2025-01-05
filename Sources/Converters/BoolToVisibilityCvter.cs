using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Activator.Converters
{
    public class BoolToVisibilityCvter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool boolValue && boolValue) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is Visibility visibilityValue) && (visibilityValue == Visibility.Visible);
        }
    }
}
