using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KMS_Activator
{
    public class EnlargeGridSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double sourceValue = (double)value;
                return sourceValue * 2 - 25.0;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double targetValue = (double)value;
                return (targetValue + 25.0) / 2;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
