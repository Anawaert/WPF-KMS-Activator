using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Activator.Converters
{
    /// <summary>
    /// <para><see langword="bool"/> 值到 <see cref="Visibility"/> 值的转换器，加入了对 Markup 语法的支持，且支持调整 Hidden 和 Collapsed</para>
    /// <para>借鉴自 十月的寒流 ，原文地址：<see href="https://blog.coldwind.top/posts/valueconverter-tips-and-tricks/"/></para>
    /// <para>Converts a <see langword="bool"/> value to a <see cref="Visibility"/> value, with support for Markup syntax and Hidden and Collapsed</para>
    /// <para>Inspired by Dr.ColdWind, original address: <see href="https://blog.coldwind.top/posts/valueconverter-tips-and-tricks/"/></para>
    /// </summary>
    public class BoolToVisibilityCvter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// <para>是否启用反向转换</para>
        /// <para>Whether to enable reverse conversion</para>
        /// </summary>
        public bool IsReversed { get; set; }

        /// <summary>
        /// <para>是否使用 Hidden 而不是 Collapsed</para>
        /// <para>Whether to use Hidden instead of Collapsed</para>
        /// </summary>
        public bool IsHiddenInsteadOfCollapsed { get; set; }

        /// <summary>
        /// <para>从 <see langword="bool"/> 值转换为 <see cref="Visibility"/> 值</para>
        /// <para>Converts from <see langword="bool"/> value to <see cref="Visibility"/> value</para>
        /// </summary>
        /// <param name="value">
        /// <para>要转换的 <see langword="bool"/> 值</para>
        /// <para>The <see langword="bool"/> value to convert</para>
        /// </param>
        /// <param name="targetType">
        /// <para>要转换到的类型</para>
        /// <para>The type to convert to</para>
        /// </param>
        /// <param name="parameter">
        /// <para>可选的转换参数</para>
        /// <para>Optional conversion parameter</para>
        /// </param>
        /// <param name="culture">
        /// <para>转换的区域性信息</para>
        /// <para>Cultural information for conversion</para>
        /// </param>
        /// <returns>
        /// <para>转换后的 <see cref="Visibility"/> 值</para>
        /// <para>The converted <see cref="Visibility"/> value</para>
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                boolValue = IsReversed ? !boolValue : boolValue;
                return boolValue ? Visibility.Visible : (IsHiddenInsteadOfCollapsed ? Visibility.Hidden : Visibility.Collapsed);
            }
            return Binding.DoNothing;
        }

        /// <summary>
        /// <para>从 <see cref="Visibility"/> 值转换为 <see langword="bool"/> 值</para>
        /// <para>Converts from <see cref="Visibility"/> value to <see langword="bool"/> value</para>
        /// </summary>
        /// <param name="value">
        /// <para>要转换的 <see cref="Visibility"/> 值</para>
        /// <para>The <see cref="Visibility"/> value to convert</para>
        /// </param>
        /// <param name="targetType">
        /// <para>要转换到的类型</para>
        /// <para>The type to convert to</para>
        /// </param>
        /// <param name="parameter">
        /// <para>可选的转换参数</para>
        /// <para>Optional conversion parameter</para>
        /// </param>
        /// <param name="culture">
        /// <para>转换的区域性信息</para>
        /// <para>Cultural information for conversion</para>
        /// </param>
        /// <returns>
        /// <para>转换后的 <see langword="bool"/> 值</para>
        /// <para>The converted <see langword="bool"/> value</para>
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return IsReversed ? visibility != Visibility.Visible : visibility == Visibility.Visible;
            }
            return Binding.DoNothing;
        }

        /// <summary>
        /// <para>提供服务</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>服务提供者</para>
        /// <para>Service provider</para>
        /// </param>
        /// <returns>
        /// <para>返回 <see cref="BoolToVisibilityCvter"/> 自身实例</para>
        /// <para>Returns the instance of <see cref="BoolToVisibilityCvter"/> itself</para>
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
