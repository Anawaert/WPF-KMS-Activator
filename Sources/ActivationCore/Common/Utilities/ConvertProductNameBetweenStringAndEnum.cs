using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activator
{
    public partial class Utility
    {
        public static string ConvertWindowsProductNameToString(WindowsProductName productName) => productName.ToString().Replace("_Point_", ".").Replace("_", " ");

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

        public static string ConvertOfficeEditionToString(OfficeEditionName edition) => edition.ToString().Replace("_", " ");

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
