using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Activator.Converters
{
    /// <summary>
    /// <para>将多个 <see langword="bool"/> 值转为一个 <see langword="bool"/> 值，当其中任意一个值为 <see langword="true"/> 时，返回 <see langword="false"/></para>
    /// <para>该转换器主要用于当执行一个或多个异步命令时，可以控制多个按钮的可用性</para>
    /// <para>Convert multiple <see langword="bool"/> values to a single <see langword="bool"/> value. When any of them is <see langword="true"/>, return <see langword="false"/></para>
    /// <para>This converter is mainly used to control the availability of multiple buttons when executing one or more asynchronous commands</para>
    /// </summary>
    public class IsRunningToIsEnabledMultiValueCvter : IMultiValueConverter
    {
        /// <summary>
        /// <para>将多个 <see langword="bool"/> 值转为一个 <see langword="bool"/> 值，当其中任意一个值为 <see langword="true"/> 时，返回 <see langword="false"/></para>
        /// <para>Convert multiple <see langword="bool"/> values to a single <see langword="bool"/> value. When any of them is <see langword="true"/>, return <see langword="false"/></para>
        /// </summary>
        /// <param name="values">
        /// <para>要转换的值的数组，元素应该是 <see langword="bool"/> 类型</para>
        /// <para>An array of values to convert, the elements should be of type <see langword="bool"/></para>
        /// </param>
        /// <param name="targetType">
        /// <para>要转换的值的类型</para>
        /// <para>The type of value to convert</para>
        /// </param>
        /// <param name="parameter">
        /// <para>可选的参数</para>
        /// <para>Optional parameter</para>
        /// </param>
        /// <param name="culture">
        /// <para>要用于转换的区域性</para>
        /// <para>The culture to use in the conversion</para>
        /// </param>
        /// <returns>
        /// <para>一个 <see langword="true"/> 值，对应可用性</para>
        /// <para>A <see langword="true"/> value, corresponding to availability</para>
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<bool> boolValues = values.Cast<bool>();
            foreach (bool value in boolValues)
            {
                if (value)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// <para>将 <see langword="bool"/> 值转为多个 <see langword="bool"/> 值，不支持此操作</para>
        /// <para>Convert <see langword="bool"/> value to multiple <see langword="bool"/> values, this operation is not supported</para>
        /// </summary>
        /// <param name="value">
        /// <para>要转换的 <see langword="bool"/> 类型值</para>
        /// <para>The <see langword="bool"/> type value to convert</para>
        /// </param>
        /// <param name="targetTypes">
        /// <para>要转换的值的类型数组</para>
        /// <para>An array of types to convert</para>
        /// </param>
        /// <param name="parameter">
        /// <para>可选的参数</para>
        /// <para>Optional parameter</para>
        /// </param>
        /// <param name="culture">
        /// <para>要用于转换的区域性</para>
        /// <para>The culture to use in the conversion</para>
        /// </param>
        /// <returns>
        /// <para>一个 <see langword="true"/> 值数组，对应多个源 <see langword="bool"/> 类型值</para>
        /// <para>An array of <see langword="true"/> values, corresponding to multiple source <see langword="bool"/> type values</para>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// <para>暂未实现此方法</para>
        /// <para>This method is not implemented yet</para>
        /// </exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
