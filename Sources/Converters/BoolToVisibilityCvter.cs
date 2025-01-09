using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Activator.Converters
{
    public class BoolToVisibilityCvter : MarkupExtension, IValueConverter
    {
        public bool IsReversed { get; set; }

        public bool IsHiddenInsteadOfCollapsed { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                boolValue = IsReversed ? !boolValue : boolValue;
                return boolValue ? Visibility.Visible : (IsHiddenInsteadOfCollapsed ? Visibility.Hidden : Visibility.Collapsed);
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return IsReversed ? visibility != Visibility.Visible : visibility == Visibility.Visible;
            }
            return Binding.DoNothing;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
