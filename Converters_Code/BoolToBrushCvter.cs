using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace KMS_Activator.Converters
{
    /// <summary>
    ///     <para>
    ///         该类主要提供一个由bool值对Brushes类型的转换器
    ///     </para>
    ///     <para>
    ///         
    ///     </para>
    /// </summary>
    public class BoolToBrushCvter : IValueConverter
    {
        /// <summary>
        ///     <para>
        ///         转换函数，详见接口<see cref="IValueConverter"/>中对该函数的注释
        ///     </para>
        ///     <para>
        ///         
        ///     </para>        
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (booleanValue)
                {
                    return Brushes.Black;
                }
                else
                {
                    return Brushes.White;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        /// <summary>
        ///     <para>
        ///         反向转换函数，详见接口<see cref="IValueConverter"/>中对该函数的注释
        ///     </para>
        ///     <para>
        ///         
        ///     </para>        
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
