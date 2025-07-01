using System;

namespace Activator
{
    public partial class Utility
    {
        /// <summary>
        /// <para>将 <see cref="WindowsProductName"/> 枚举转换为对应的 <see langword="string"/> 字符串</para>
        /// <para>Convert <see cref="WindowsProductName"/> enum to corresponding <see langword="string"/></para>
        /// </summary>
        /// <param name="productName">
        /// <para>需要转换的 <see cref="WindowsProductName"/> 枚举</para>
        /// <para><see cref="WindowsProductName"/> enum to convert</para>
        /// </param>
        /// <returns>
        /// <para>转换后的 <see langword="string"/> 字符串</para>
        /// <para>Converted <see langword="string"/></para>
        /// </returns>
        public static string ConvertWindowsProductNameToString(WindowsProductName productName) => productName.ToString().Replace("_Point_", ".").Replace("_", " ");

        /// <summary>
        /// <para>将 <see langword="string"/> 字符串转换为对应的 <see cref="WindowsProductName"/> 枚举</para>
        /// <para>Convert <see langword="string"/> to corresponding <see cref="WindowsProductName"/> enum</para>
        /// </summary>
        /// <param name="productName">
        /// <para>需要转换的 <see langword="string"/> 字符串</para>
        /// <para><see langword="string"/> to convert</para>
        /// </param>
        /// <returns>
        /// <para>转换后的 <see cref="WindowsProductName"/> 枚举，若传入的 <see langword="string"/> 字符串没有对应的产品名称，则返回 <see cref="WindowsProductName.Unsupported"/></para>
        /// <para>Converted <see cref="WindowsProductName"/> enum, if the <see langword="string"/> does not have a corresponding product name, return <see cref="WindowsProductName.Unsupported"/></para>
        /// </returns>
        public static WindowsProductName ConvertWindowsProductNameToEnum(string productName)
        {
            foreach (WindowsProductName product in Enum.GetValues(typeof(WindowsProductName)))
            {
                string productString = ConvertWindowsProductNameToString(product);
                if (productName.Contains(productString) || productString.Contains(productName))
                {
                    return product;
                }
            }
            return WindowsProductName.Unsupported;
        }

        /// <summary>
        /// <para>将 <see cref="OfficeEditionName"/> 枚举转换为对应的 <see langword="string"/> 字符串</para>
        /// <para>Convert <see cref="OfficeEditionName"/> enum to corresponding <see langword="string"/></para>
        /// </summary>
        /// <param name="edition">
        /// <para>需要转换的 <see cref="OfficeEditionName"/> 枚举</para>
        /// <para><see cref="OfficeEditionName"/> enum to convert</para>
        /// </param>
        /// <returns>
        /// <para>转换后的 <see langword="string"/> 字符串</para>
        /// <para>Converted <see langword="string"/></para>
        /// </returns>
        public static string ConvertOfficeEditionToString(OfficeEditionName edition) => edition.ToString().Replace("_", " ");

        /// <summary>
        /// <para>将 <see langword="string"/> 字符串转换为对应的 <see cref="OfficeEditionName"/> 枚举</para>
        /// <para>Convert <see langword="string"/> to corresponding <see cref="OfficeEditionName"/> enum</para>
        /// </summary>
        /// <param name="edition">
        /// <para>需要转换的 <see langword="string"/> 字符串</para>
        /// <para><see langword="string"/> to convert</para>
        /// </param>
        /// <returns>
        /// <para>转换后的 <see cref="OfficeEditionName"/> 枚举，若传入的 <see langword="string"/> 字符串没有对应的产品名称，则返回 <see cref="OfficeEditionName.Unsupported"/></para>
        /// <para>Converted <see cref="OfficeEditionName"/> enum, if the <see langword="string"/> does not have a corresponding product name, return <see cref="OfficeEditionName.Unsupported"/></para>
        /// </returns>
        public static OfficeEditionName ConvertOfficeEditionToEnum(string edition)
        {
            foreach (OfficeEditionName product in Enum.GetValues(typeof(OfficeEditionName)))
            {
                string productString = ConvertOfficeEditionToString(product);
                if (edition.Contains(productString) || productString.Contains(edition))
                {
                    return product;
                }
            }
            return OfficeEditionName.Unsupported;
        }
    }
}
